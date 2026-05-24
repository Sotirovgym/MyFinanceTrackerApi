import { CurrencyCode, CurrencyIsoCode } from '@/shared/enums/currencyCode';

const formatters = new Map<string, Intl.NumberFormat>();

function getFormatter(currency: CurrencyCode): Intl.NumberFormat {
  const iso = CurrencyIsoCode[currency];
  let formatter = formatters.get(iso);
  if (!formatter) {
    formatter = new Intl.NumberFormat(undefined, {
      style: 'currency',
      currency: iso,
      currencyDisplay: 'symbol',
    });
    formatters.set(iso, formatter);
  }
  return formatter;
}

export function formatMoney(amount: number, currency: CurrencyCode): string {
  return getFormatter(currency).format(amount);
}

export function formatSignedMoney(
  amount: number,
  currency: CurrencyCode,
  signed: boolean,
): string {
  if (!signed) {
    return formatMoney(amount, currency);
  }

  const formatted = formatMoney(Math.abs(amount), currency);
  return amount < 0 ? `-${formatted}` : `+${formatted}`;
}
