import type { TransactionType } from '@/shared/enums/transactionType';

export type Transaction = {
  id: string;
  accountId: string;
  categoryId: string;
  type: TransactionType;
  amount: number;
  transactionDate: string;
  description?: string | null;
  createdAt: string;
  updatedAt: string;
};

export type CreateTransactionRequest = {
  accountId: string;
  categoryId: string;
  type: TransactionType;
  amount: number;
  transactionDate: string;
  description?: string | null;
};

export type UpdateTransactionRequest = CreateTransactionRequest;

export type TransactionFilter = {
  accountId?: string;
  categoryId?: string;
  from?: string;
  to?: string;
};
