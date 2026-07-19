import { useMemo } from 'react';
import { MoreHorizontalIcon, PencilIcon, Trash2Icon } from 'lucide-react';

import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
  Table,
  TableBody,
  TableCell,
  TableFooter,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';

import { CurrencyCode } from '@/shared/enums/currencyCode';
import { TransactionType } from '@/shared/enums/transactionType';
import { formatDate } from '@/shared/utils/date';
import { formatMoney } from '@/shared/utils/money';
import { cn } from '@/lib/utils';

import type { Account } from '@/features/accounts/types';
import type { Category } from '@/features/categories/types';
import type { Transaction } from '../types';

type CurrencyTotals = {
  income: number;
  expense: number;
  net: number;
}

type TransactionsTableProps = {
  transactions: Transaction[];
  accounts: Account[];
  categories: Category[];
  onEdit: (transaction: Transaction) => void;
  onDelete: (transaction: Transaction) => void;
};

export function TransactionsTable({
  transactions,
  accounts,
  categories,
  onEdit,
  onDelete,
}: TransactionsTableProps) {
  const accountById = useMemo(() => new Map(accounts.map((a) => [a.id, a])), [accounts]);
  const categoryById = useMemo(() => new Map(categories.map((c) => [c.id, c])), [categories]);

  const totalsByCurrency = useMemo(() => {
    const map = new Map<CurrencyCode, CurrencyTotals>();

    for (const transaction of transactions) {
      const account = accountById.get(transaction.accountId);
      const currency = account?.currency ?? CurrencyCode.USD;

      let totals = map.get(currency);
      if (!totals){
        totals = { income: 0, expense: 0, net: 0 };
        map.set(currency, totals);
      }

      if (transaction.type === TransactionType.Income) {
        totals.income += transaction.amount;
        totals.net += transaction.amount;
      } else {
        totals.expense += transaction.amount;
        totals.net -= transaction.amount;
      }
    }

    return map;
  }, [transactions, accountById]);

  return (
    <div className="border-border rounded-lg border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Date</TableHead>
            <TableHead>Description</TableHead>
            <TableHead>Account</TableHead>
            <TableHead>Category</TableHead>
            <TableHead className="text-right">Amount</TableHead>
            <TableHead className="w-12" />
          </TableRow>
        </TableHeader>
        <TableBody>
          {transactions.map((transaction) => {
            const account = accountById.get(transaction.accountId);
            const category = categoryById.get(transaction.categoryId);
            const currency = account?.currency ?? CurrencyCode.USD;
            const isIncome = transaction.type === TransactionType.Income;

            return (
              <TableRow key={transaction.id}>
                <TableCell className="whitespace-nowrap">
                  {formatDate(transaction.transactionDate)}
                </TableCell>
                <TableCell className="max-w-[280px] truncate">
                  {transaction.description ?? (
                    <span className="text-muted-foreground italic">No description</span>
                  )}
                </TableCell>
                <TableCell>{account?.name ?? <span className="text-muted-foreground">Unknown</span>}</TableCell>
                <TableCell>
                  {category ? (
                    <Badge variant={isIncome ? 'secondary' : 'outline'}>{category.name}</Badge>
                  ) : (
                    <span className="text-muted-foreground">Unknown</span>
                  )}
                </TableCell>
                <TableCell
                  className={cn(
                    'text-right font-medium tabular-nums',
                    isIncome ? 'text-emerald-600 dark:text-emerald-400' : 'text-foreground',
                  )}
                >
                  {isIncome ? '+' : '-'}
                  {formatMoney(transaction.amount, currency)}
                </TableCell>
                <TableCell className="text-right">
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant="ghost" size="icon" aria-label="Open actions">
                        <MoreHorizontalIcon />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      <DropdownMenuItem onSelect={() => onEdit(transaction)}>
                        <PencilIcon />
                        Edit
                      </DropdownMenuItem>
                      <DropdownMenuItem
                        variant="destructive"
                        onSelect={() => onDelete(transaction)}
                      >
                        <Trash2Icon />
                        Delete
                      </DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
        <TableFooter>
          {[...totalsByCurrency.entries()].map(([currency, totals]) => (
        <TableRow key={currency}>
          <TableCell colSpan={4} className="text-right font-semibold">
            {totalsByCurrency.size > 1 ? `Total (${currency})` : 'Total'}
          </TableCell>
          <TableCell
            className={cn(
              'text-right font-semibold tabular-nums',
              totals.net >= 0 ? 'text-emerald-600 dark:text-emerald-400' : 'text-red-600 dark:text-red-400',
            )}
          >
            {totals.net >= 0 ? '+' : ''}
            {formatMoney(Math.abs(totals.net), currency)}
          </TableCell>
          <TableCell />
        </TableRow>
      ))}
    </TableFooter>
      </Table>
    </div>
  );
}
