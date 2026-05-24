export const CurrencyCode = {
  USD: 0,
  EUR: 1,
} as const;

export type CurrencyCode = (typeof CurrencyCode)[keyof typeof CurrencyCode];

export const CurrencyCodeLabels: Record<CurrencyCode, string> = {
  [CurrencyCode.USD]: 'USD',
  [CurrencyCode.EUR]: 'EUR',
};

export const CurrencyIsoCode: Record<CurrencyCode, string> = {
  [CurrencyCode.USD]: 'USD',
  [CurrencyCode.EUR]: 'EUR',
};

export const CurrencyCodeOptions = (Object.values(CurrencyCode) as CurrencyCode[]).map(
  (value) => ({
    value,
    label: CurrencyCodeLabels[value],
  }),
);
