import type { TransactionType } from '@/shared/enums/transactionType';

export type Category = {
  id: string;
  name: string;
  type: TransactionType;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
};

export type CreateCategoryRequest = {
  name: string;
  type: TransactionType;
};

export type UpdateCategoryRequest = {
  name: string;
  type: TransactionType;
  isActive: boolean;
};
