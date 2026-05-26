import { useMemo, useState } from 'react';
import { PlusIcon } from 'lucide-react';
import { toast } from 'sonner';

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';

import { PageHeader } from '@/shared/components/PageHeader';
import { AsyncBoundary } from '@/shared/components/AsyncBoundary';
import { ConfirmDialog } from '@/shared/components/ConfirmDialog';

import { useAccounts } from '@/features/accounts/api';
import { useCategories } from '@/features/categories/api';

import { useDeleteTransaction, useTransactions } from '../api';
import { TransactionFilters } from '../components/TransactionFilters';
import { TransactionFormDialog } from '../components/TransactionFormDialog';
import { TransactionsTable } from '../components/TransactionsTable';
import { useTransactionFilters } from '../useTransactionFilters';
import type { Transaction } from '../types';

export default function TransactionsPage() {
  const [formOpen, setFormOpen] = useState(false);
  const [selected, setSelected] = useState<Transaction | undefined>(undefined);
  const [toDelete, setToDelete] = useState<Transaction | null>(null);

  const { apiFilter } = useTransactionFilters();
  const transactionsQuery = useTransactions(apiFilter);
  const accountsQuery = useAccounts(true);
  const categoriesQuery = useCategories(true);
  const deleteTransaction = useDeleteTransaction();

  const transactions = useMemo(
    () =>
      [...(transactionsQuery.data ?? [])].sort(
        (a, b) => Date.parse(b.transactionDate) - Date.parse(a.transactionDate),
      ),
    [transactionsQuery.data],
  );

  const handleCreate = () => {
    setSelected(undefined);
    setFormOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (!toDelete) {
      return;
    }
    try {
      await deleteTransaction.mutateAsync(toDelete.id);
      toast.success('Transaction deleted');
      setToDelete(null);
    } catch (error) {
      toast.error(error instanceof Error ? error.message : 'Failed to delete transaction');
    }
  };

  const hasAccounts = (accountsQuery.data?.length ?? 0) > 0;
  const hasCategories = (categoriesQuery.data?.length ?? 0) > 0;
  const canCreate = hasAccounts && hasCategories;

  return (
    <div className="space-y-6">
      <PageHeader
        title="Transactions"
        description="Every income and expense, filtered by account, category, or date."
        actions={
          <Button onClick={handleCreate} disabled={!canCreate}>
            <PlusIcon />
            New transaction
          </Button>
        }
      />

      <TransactionFilters />

      <AsyncBoundary
        isPending={transactionsQuery.isPending}
        error={transactionsQuery.error}
        isEmpty={transactions.length === 0}
        emptyTitle={canCreate ? 'No transactions found' : 'Add accounts and categories first'}
        emptyDescription={
          canCreate
            ? 'Adjust the filters above or record your first transaction.'
            : 'You need at least one account and one category before you can record transactions.'
        }
        emptyAction={
          canCreate ? (
            <Button onClick={handleCreate}>
              <PlusIcon />
              New transaction
            </Button>
          ) : undefined
        }
        loadingFallback={
          <div className="space-y-2">
            <Skeleton className="h-10 w-full" />
            <Skeleton className="h-10 w-full" />
            <Skeleton className="h-10 w-full" />
          </div>
        }
      >
        <TransactionsTable
          transactions={transactions}
          accounts={accountsQuery.data ?? []}
          categories={categoriesQuery.data ?? []}
          onEdit={(transaction) => {
            setSelected(transaction);
            setFormOpen(true);
          }}
          onDelete={(transaction) => setToDelete(transaction)}
        />
      </AsyncBoundary>

      <TransactionFormDialog
        open={formOpen}
        onOpenChange={(open) => {
          setFormOpen(open);
          if (!open) {
            setSelected(undefined);
          }
        }}
        transaction={selected}
      />

      <ConfirmDialog
        open={toDelete !== null}
        onOpenChange={(open) => {
          if (!open) {
            setToDelete(null);
          }
        }}
        title="Delete transaction?"
        description="This cannot be undone."
        confirmLabel="Delete"
        destructive
        loading={deleteTransaction.isPending}
        onConfirm={handleConfirmDelete}
      />
    </div>
  );
}
