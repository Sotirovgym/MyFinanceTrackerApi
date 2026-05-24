import { z } from 'zod';
import { TransactionType } from '@/shared/enums/transactionType';

const transactionTypeField = z.union(
  [z.literal(TransactionType.Income), z.literal(TransactionType.Expense)],
  { message: 'Pick a type' },
);

export const createCategorySchema = z.object({
  name: z.string().trim().min(1, 'Name is required').max(100, 'Name is too long'),
  type: transactionTypeField,
});

export type CreateCategoryFormValues = z.infer<typeof createCategorySchema>;

export const updateCategorySchema = z.object({
  name: z.string().trim().min(1, 'Name is required').max(100, 'Name is too long'),
  type: transactionTypeField,
  isActive: z.boolean(),
});

export type UpdateCategoryFormValues = z.infer<typeof updateCategorySchema>;
