import { useCallback, useMemo } from 'react';
import { useSearchParams } from 'react-router';
import { startOfDayUtc, endOfDayUtc } from '@/shared/utils/date';
import type { TransactionFilter } from './types';

const QUERY_KEYS = {
  accountId: 'accountId',
  categoryId: 'categoryId',
  from: 'from',
  to: 'to',
} as const;

export type TransactionFilterState = {
  accountId?: string;
  categoryId?: string;
  from?: Date;
  to?: Date;
};

function parseDate(value: string | null): Date | undefined {
  if (!value) {
    return undefined;
  }
  const ms = Date.parse(value);
  return Number.isNaN(ms) ? undefined : new Date(ms);
}

export function useTransactionFilters() {
  const [searchParams, setSearchParams] = useSearchParams();

  const state: TransactionFilterState = useMemo(
    () => ({
      accountId: searchParams.get(QUERY_KEYS.accountId) ?? undefined,
      categoryId: searchParams.get(QUERY_KEYS.categoryId) ?? undefined,
      from: parseDate(searchParams.get(QUERY_KEYS.from)),
      to: parseDate(searchParams.get(QUERY_KEYS.to)),
    }),
    [searchParams],
  );

  const apiFilter: TransactionFilter = useMemo(
    () => ({
      accountId: state.accountId,
      categoryId: state.categoryId,
      from: state.from ? startOfDayUtc(state.from).toISOString() : undefined,
      to: state.to ? endOfDayUtc(state.to).toISOString() : undefined,
    }),
    [state],
  );

  const setFilter = useCallback(
    (next: Partial<TransactionFilterState>) => {
      setSearchParams(
        (current) => {
          const params = new URLSearchParams(current);
          const merged: TransactionFilterState = { ...state, ...next };

          if (merged.accountId) {
            params.set(QUERY_KEYS.accountId, merged.accountId);
          } else {
            params.delete(QUERY_KEYS.accountId);
          }

          if (merged.categoryId) {
            params.set(QUERY_KEYS.categoryId, merged.categoryId);
          } else {
            params.delete(QUERY_KEYS.categoryId);
          }

          if (merged.from) {
            params.set(QUERY_KEYS.from, merged.from.toISOString());
          } else {
            params.delete(QUERY_KEYS.from);
          }

          if (merged.to) {
            params.set(QUERY_KEYS.to, merged.to.toISOString());
          } else {
            params.delete(QUERY_KEYS.to);
          }

          return params;
        },
        { replace: true },
      );
    },
    [setSearchParams, state],
  );

  const clear = useCallback(() => {
    setSearchParams(new URLSearchParams(), { replace: true });
  }, [setSearchParams]);

  const hasAny =
    state.accountId !== undefined ||
    state.categoryId !== undefined ||
    state.from !== undefined ||
    state.to !== undefined;

  return { state, apiFilter, setFilter, clear, hasAny };
}
