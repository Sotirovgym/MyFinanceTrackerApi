import { useMemo } from 'react';
import { XIcon } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';

import { Combobox, type ComboboxOption } from '@/shared/forms/Combobox';
import { DatePicker } from '@/shared/forms/DatePicker';

import { useAccounts } from '@/features/accounts/api';
import { useCategories } from '@/features/categories/api';

import { useTransactionFilters } from '../useTransactionFilters';

export function TransactionFilters() {
  const { state, setFilter, clear, hasAny } = useTransactionFilters();
  const accountsQuery = useAccounts(true);
  const categoriesQuery = useCategories(true);

  const accountOptions: ComboboxOption<string>[] = useMemo(
    () =>
      (accountsQuery.data ?? []).map((account) => ({
        value: account.id,
        label: account.name,
      })),
    [accountsQuery.data],
  );

  const categoryOptions: ComboboxOption<string>[] = useMemo(
    () =>
      (categoriesQuery.data ?? []).map((category) => ({
        value: category.id,
        label: category.name,
      })),
    [categoriesQuery.data],
  );

  return (
    <div className="border-border bg-card rounded-lg border p-4">
      <div className="grid gap-3 sm:grid-cols-2 lg:grid-cols-4">
        <div className="space-y-1.5">
          <Label htmlFor="filter-account">Account</Label>
          <Combobox
            id="filter-account"
            value={state.accountId}
            onChange={(value) => setFilter({ accountId: value })}
            options={accountOptions}
            placeholder="All accounts"
            searchPlaceholder="Search accounts..."
          />
        </div>

        <div className="space-y-1.5">
          <Label htmlFor="filter-category">Category</Label>
          <Combobox
            id="filter-category"
            value={state.categoryId}
            onChange={(value) => setFilter({ categoryId: value })}
            options={categoryOptions}
            placeholder="All categories"
            searchPlaceholder="Search categories..."
          />
        </div>

        <div className="space-y-1.5">
          <Label htmlFor="filter-from">From</Label>
          <DatePicker
            id="filter-from"
            value={state.from}
            onChange={(value) => setFilter({ from: value })}
            placeholder="Any"
          />
        </div>

        <div className="space-y-1.5">
          <Label htmlFor="filter-to">To</Label>
          <DatePicker
            id="filter-to"
            value={state.to}
            onChange={(value) => setFilter({ to: value })}
            placeholder="Any"
          />
        </div>
      </div>

      {hasAny ? (
        <div className="mt-3 flex justify-end">
          <Button variant="ghost" size="sm" onClick={clear}>
            <XIcon />
            Clear filters
          </Button>
        </div>
      ) : null}
    </div>
  );
}
