import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '@/shared/api/client';
import type {
  Account,
  CreateAccountRequest,
  UpdateAccountRequest,
} from './types';

export const accountsKeys = {
  all: ['accounts'] as const,
  lists: () => [...accountsKeys.all, 'list'] as const,
  list: (includeInactive: boolean) =>
    [...accountsKeys.lists(), { includeInactive }] as const,
  details: () => [...accountsKeys.all, 'detail'] as const,
  detail: (id: string) => [...accountsKeys.details(), id] as const,
};

export function useAccounts(includeInactive = false) {
  return useQuery({
    queryKey: accountsKeys.list(includeInactive),
    queryFn: ({ signal }) =>
      api.get<Account[]>('/api/Accounts', { query: { includeInactive }, signal }),
  });
}

export function useAccount(id: string | undefined) {
  return useQuery({
    queryKey: accountsKeys.detail(id ?? ''),
    queryFn: ({ signal }) => api.get<Account>(`/api/Accounts/${id}`, { signal }),
    enabled: Boolean(id),
  });
}

export function useCreateAccount() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: CreateAccountRequest) =>
      api.post<Account>('/api/Accounts', request),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: accountsKeys.lists() });
    },
  });
}

export function useUpdateAccount() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, request }: { id: string; request: UpdateAccountRequest }) =>
      api.put<Account>(`/api/Accounts/${id}`, request),
    onSuccess: (_data, variables) => {
      void queryClient.invalidateQueries({ queryKey: accountsKeys.lists() });
      void queryClient.invalidateQueries({ queryKey: accountsKeys.detail(variables.id) });
    },
  });
}

export function useDeleteAccount() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => api.del(`/api/Accounts/${id}`),
    onSuccess: (_data, id) => {
      void queryClient.invalidateQueries({ queryKey: accountsKeys.lists() });
      queryClient.removeQueries({ queryKey: accountsKeys.detail(id) });
    },
  });
}
