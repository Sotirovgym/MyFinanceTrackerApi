import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '@/shared/api/client';
import type { Category, CreateCategoryRequest, UpdateCategoryRequest } from './types';

export const categoriesKeys = {
  all: ['categories'] as const,
  lists: () => [...categoriesKeys.all, 'list'] as const,
  list: (includeInactive: boolean) =>
    [...categoriesKeys.lists(), { includeInactive }] as const,
  details: () => [...categoriesKeys.all, 'detail'] as const,
  detail: (id: string) => [...categoriesKeys.details(), id] as const,
};

export function useCategories(includeInactive = false) {
  return useQuery({
    queryKey: categoriesKeys.list(includeInactive),
    queryFn: ({ signal }) =>
      api.get<Category[]>('/api/Categories', { query: { includeInactive }, signal }),
  });
}

export function useCategory(id: string | undefined) {
  return useQuery({
    queryKey: categoriesKeys.detail(id ?? ''),
    queryFn: ({ signal }) => api.get<Category>(`/api/Categories/${id}`, { signal }),
    enabled: Boolean(id),
  });
}

export function useCreateCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: CreateCategoryRequest) =>
      api.post<Category>('/api/Categories', request),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: categoriesKeys.lists() });
    },
  });
}

export function useUpdateCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, request }: { id: string; request: UpdateCategoryRequest }) =>
      api.put<Category>(`/api/Categories/${id}`, request),
    onSuccess: (_data, variables) => {
      void queryClient.invalidateQueries({ queryKey: categoriesKeys.lists() });
      void queryClient.invalidateQueries({ queryKey: categoriesKeys.detail(variables.id) });
    },
  });
}

export function useDeleteCategory() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => api.del(`/api/Categories/${id}`),
    onSuccess: (_data, id) => {
      void queryClient.invalidateQueries({ queryKey: categoriesKeys.lists() });
      queryClient.removeQueries({ queryKey: categoriesKeys.detail(id) });
    },
  });
}
