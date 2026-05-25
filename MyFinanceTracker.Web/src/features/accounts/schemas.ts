import { z } from 'zod';
import { AccountType } from '@/shared/enums/accountType';
import { CurrencyCode } from '@/shared/enums/currencyCode';

const accountTypeField = z.union(
  [
    z.literal(AccountType.Checking),
    z.literal(AccountType.Savings),
    z.literal(AccountType.CreditCard),
    z.literal(AccountType.Cash),
  ],
  { message: 'Pick an account type' },
);

const currencyField = z.union(
  [z.literal(CurrencyCode.USD), z.literal(CurrencyCode.EUR)],
  { message: 'Pick a currency' },
);

export const createAccountSchema = z.object({
  name: z.string().trim().min(1, 'Name is required').max(100, 'Name is too long'),
  accountType: accountTypeField,
  currency: currencyField,
  initialBalance: z
    .number({ message: 'Enter a number' })
    .finite('Enter a valid number'),
});

export type CreateAccountFormValues = z.infer<typeof createAccountSchema>;

export const updateAccountSchema = z.object({
  name: z.string().trim().min(1, 'Name is required').max(100, 'Name is too long'),
  accountType: accountTypeField,
  currency: currencyField,
  isActive: z.boolean(),
});

export type UpdateAccountFormValues = z.infer<typeof updateAccountSchema>;
