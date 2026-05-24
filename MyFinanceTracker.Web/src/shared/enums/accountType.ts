export const AccountType = {
  Checking: 0,
  Savings: 1,
  CreditCard: 2,
  Cash: 3,
} as const;

export type AccountType = (typeof AccountType)[keyof typeof AccountType];

export const AccountTypeLabels: Record<AccountType, string> = {
  [AccountType.Checking]: 'Checking',
  [AccountType.Savings]: 'Savings',
  [AccountType.CreditCard]: 'Credit card',
  [AccountType.Cash]: 'Cash',
};

export const AccountTypeOptions = (Object.values(AccountType) as AccountType[]).map((value) => ({
  value,
  label: AccountTypeLabels[value],
}));
