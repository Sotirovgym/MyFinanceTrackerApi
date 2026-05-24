import type { AccountType } from '@/shared/enums/accountType';
import type { CurrencyCode } from '@/shared/enums/currencyCode';

export type Account = {
  id: string;
  name: string;
  accountType: AccountType;
  currency: CurrencyCode;
  balance: number;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
};

export type CreateAccountRequest = {
  name: string;
  accountType: AccountType;
  currency: CurrencyCode;
  initialBalance: number;
};

export type UpdateAccountRequest = {
  name: string;
  accountType: AccountType;
  currency: CurrencyCode;
  isActive: boolean;
};
