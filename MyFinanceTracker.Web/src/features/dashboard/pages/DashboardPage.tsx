import { useMemo } from 'react';
import { Link } from 'react-router';
import { ArrowDownIcon, ArrowUpIcon, PlusIcon, WalletIcon } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Skeleton } from '@/components/ui/skeleton';

import { PageHeader } from '@/shared/components/PageHeader';
import { CurrencyCode, CurrencyCodeLabels } from '@/shared/enums/currencyCode';
import { TransactionType } from '@/shared/enums/transactionType';
import { formatDate } from '@/shared/utils/date';
import { formatMoney } from '@/shared/utils/money';
import { cn } from '@/lib/utils';

import { useAccounts } from '@/features/accounts/api';
import { useCategories } from '@/features/categories/api';
import { useTransactions } from '@/features/transactions/api';

function startOfThisMonthIso(): string {
  const now = new Date();
  return new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1, 0, 0, 0, 0)).toISOString();
}

function endOfThisMonthIso(): string {
  const now = new Date();
  return new Date(
    Date.UTC(now.getUTCFullYear(), now.getUTCMonth() + 1, 0, 23, 59, 59, 999),
  ).toISOString();
}

export default function DashboardPage() {
  const monthRange = useMemo(
    () => ({ from: startOfThisMonthIso(), to: endOfThisMonthIso() }),
    [],
  );

  const accountsQuery = useAccounts(false);
  const categoriesQuery = useCategories(true);
  const monthTransactionsQuery = useTransactions(monthRange);
  const recentTransactionsQuery = useTransactions({});

  const totalsByCurrency = useMemo(() => {
    const totals = new Map<CurrencyCode, number>();
    for (const account of accountsQuery.data ?? []) {
      totals.set(account.currency, (totals.get(account.currency) ?? 0) + account.balance);
    }
    return Array.from(totals.entries());
  }, [accountsQuery.data]);

  const monthSummary = useMemo(() => {
    let income = 0;
    let expense = 0;
    for (const transaction of monthTransactionsQuery.data ?? []) {
      if (transaction.type === TransactionType.Income) {
        income += transaction.amount;
      } else {
        expense += transaction.amount;
      }
    }
    return { income, expense, net: income - expense };
  }, [monthTransactionsQuery.data]);

  const recent = useMemo(
    () =>
      [...(recentTransactionsQuery.data ?? [])]
        .sort((a, b) => Date.parse(b.transactionDate) - Date.parse(a.transactionDate))
        .slice(0, 5),
    [recentTransactionsQuery.data],
  );

  const accountById = useMemo(
    () => new Map((accountsQuery.data ?? []).map((a) => [a.id, a])),
    [accountsQuery.data],
  );
  const categoryById = useMemo(
    () => new Map((categoriesQuery.data ?? []).map((c) => [c.id, c])),
    [categoriesQuery.data],
  );

  return (
    <div className="space-y-6">
      <PageHeader
        title="Dashboard"
        description="Your finances at a glance."
        actions={
          <Button asChild>
            <Link to="/transactions">
              <PlusIcon />
              New transaction
            </Link>
          </Button>
        }
      />

      <section className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-sm font-medium">
              <WalletIcon className="text-muted-foreground size-4" />
              Total balance
            </CardTitle>
            <CardDescription>Across all active accounts</CardDescription>
          </CardHeader>
          <CardContent>
            {accountsQuery.isPending ? (
              <Skeleton className="h-7 w-32" />
            ) : totalsByCurrency.length === 0 ? (
              <p className="text-muted-foreground text-sm">No accounts yet.</p>
            ) : (
              <ul className="space-y-1">
                {totalsByCurrency.map(([currency, total]) => (
                  <li key={currency} className="text-2xl font-semibold tabular-nums">
                    {formatMoney(total, currency)}
                    <span className="text-muted-foreground ml-2 text-sm font-normal">
                      {CurrencyCodeLabels[currency]}
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-sm font-medium">
              <ArrowUpIcon className="size-4 text-emerald-600 dark:text-emerald-400" />
              Income this month
            </CardTitle>
            <CardDescription>Sum across all currencies</CardDescription>
          </CardHeader>
          <CardContent>
            {monthTransactionsQuery.isPending ? (
              <Skeleton className="h-7 w-32" />
            ) : (
              <p className="text-2xl font-semibold tabular-nums text-emerald-600 dark:text-emerald-400">
                +{monthSummary.income.toFixed(2)}
              </p>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-sm font-medium">
              <ArrowDownIcon className="size-4" />
              Expense this month
            </CardTitle>
            <CardDescription>Sum across all currencies</CardDescription>
          </CardHeader>
          <CardContent>
            {monthTransactionsQuery.isPending ? (
              <Skeleton className="h-7 w-32" />
            ) : (
              <p className="text-2xl font-semibold tabular-nums">
                -{monthSummary.expense.toFixed(2)}
              </p>
            )}
          </CardContent>
        </Card>
      </section>

      <Card>
        <CardHeader>
          <CardTitle>Recent transactions</CardTitle>
          <CardDescription>Your five most recent records.</CardDescription>
        </CardHeader>
        <CardContent>
          {recentTransactionsQuery.isPending ? (
            <div className="space-y-2">
              <Skeleton className="h-8 w-full" />
              <Skeleton className="h-8 w-full" />
              <Skeleton className="h-8 w-full" />
            </div>
          ) : recent.length === 0 ? (
            <p className="text-muted-foreground text-sm">No transactions yet.</p>
          ) : (
            <ul className="divide-border divide-y">
              {recent.map((transaction) => {
                const account = accountById.get(transaction.accountId);
                const category = categoryById.get(transaction.categoryId);
                const currency = account?.currency ?? CurrencyCode.USD;
                const isIncome = transaction.type === TransactionType.Income;
                return (
                  <li
                    key={transaction.id}
                    className="flex items-center justify-between py-2.5"
                  >
                    <div className="min-w-0">
                      <p className="truncate text-sm font-medium">
                        {transaction.description ?? category?.name ?? 'Transaction'}
                      </p>
                      <p className="text-muted-foreground text-xs">
                        {formatDate(transaction.transactionDate)}
                        {account ? <> &middot; {account.name}</> : null}
                        {category ? <> &middot; {category.name}</> : null}
                      </p>
                    </div>
                    <p
                      className={cn(
                        'text-sm font-medium tabular-nums',
                        isIncome ? 'text-emerald-600 dark:text-emerald-400' : 'text-foreground',
                      )}
                    >
                      {isIncome ? '+' : '-'}
                      {formatMoney(transaction.amount, currency)}
                    </p>
                  </li>
                );
              })}
            </ul>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
