export const TransactionType = {
  Income: 0,
  Expense: 1,
} as const;

export type TransactionType = (typeof TransactionType)[keyof typeof TransactionType];

export const TransactionTypeLabels: Record<TransactionType, string> = {
  [TransactionType.Income]: 'Income',
  [TransactionType.Expense]: 'Expense',
};

export const TransactionTypeOptions = (Object.values(TransactionType) as TransactionType[]).map(
  (value) => ({
    value,
    label: TransactionTypeLabels[value],
  }),
);
