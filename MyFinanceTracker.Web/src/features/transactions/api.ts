import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { api } from '@/shared/api/client';
import type {
  CreateTransactionRequest,
  Transaction,
  TransactionFilter,
  UpdateTransactionRequest,
} from './types';

export const transactionsKeys = {
  all: ['transactions'] as const,
  lists: () => [...transactionsKeys.all, 'list'] as const,
  list: (filter: TransactionFilter) => [...transactionsKeys.lists(), filter] as const,
  details: () => [...transactionsKeys.all, 'detail'] as const,
  detail: (id: string) => [...transactionsKeys.details(), id] as const,
};

export function useTransactions(filter: TransactionFilter) {
  return useQuery({
    queryKey: transactionsKeys.list(filter),
    queryFn: ({ signal }) =>
      api.get<Transaction[]>('/api/Transactions', {
        query: {
          accountId: filter.accountId,
          categoryId: filter.categoryId,
          from: filter.from,
          to: filter.to,
        },
        signal,
      }),
  });
}

export function useTransaction(id: string | undefined) {
  return useQuery({
    queryKey: transactionsKeys.detail(id ?? ''),
    queryFn: ({ signal }) => api.get<Transaction>(`/api/Transactions/${id}`, { signal }),
    enabled: Boolean(id),
  });
}

export function useCreateTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (request: CreateTransactionRequest) =>
      api.post<Transaction>('/api/Transactions', request),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: transactionsKeys.lists() });
      void queryClient.invalidateQueries({ queryKey: ['accounts', 'list'] });
    },
  });
}

export function useUpdateTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ id, request }: { id: string; request: UpdateTransactionRequest }) =>
      api.put<Transaction>(`/api/Transactions/${id}`, request),
    onSuccess: (_data, variables) => {
      void queryClient.invalidateQueries({ queryKey: transactionsKeys.lists() });
      void queryClient.invalidateQueries({ queryKey: transactionsKeys.detail(variables.id) });
      void queryClient.invalidateQueries({ queryKey: ['accounts', 'list'] });
    },
  });
}

export function useDeleteTransaction() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => api.del(`/api/Transactions/${id}`),
    onSuccess: (_data, id) => {
      void queryClient.invalidateQueries({ queryKey: transactionsKeys.lists() });
      queryClient.removeQueries({ queryKey: transactionsKeys.detail(id) });
      void queryClient.invalidateQueries({ queryKey: ['accounts', 'list'] });
    },
  });
}
