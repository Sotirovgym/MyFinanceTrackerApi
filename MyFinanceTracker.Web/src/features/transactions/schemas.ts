import { z } from 'zod';
import { TransactionType } from '@/shared/enums/transactionType';

const transactionTypeField = z.union(
  [z.literal(TransactionType.Income), z.literal(TransactionType.Expense)],
  { message: 'Pick a type' },
);

export const transactionSchema = z.object({
  accountId: z.string().uuid('Pick an account'),
  categoryId: z.string().uuid('Pick a category'),
  type: transactionTypeField,
  amount: z
    .number({ message: 'Enter an amount' })
    .positive('Amount must be greater than zero'),
  transactionDate: z.date({ message: 'Pick a date' }),
  description: z.string().max(250, 'Description is too long'),
});

export type TransactionFormValues = z.infer<typeof transactionSchema>;
