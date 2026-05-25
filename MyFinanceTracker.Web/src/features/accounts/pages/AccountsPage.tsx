import { useMemo, useState } from 'react';
import { PlusIcon } from 'lucide-react';
import { toast } from 'sonner';

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';

import { PageHeader } from '@/shared/components/PageHeader';
import { AsyncBoundary } from '@/shared/components/AsyncBoundary';
import { ConfirmDialog } from '@/shared/components/ConfirmDialog';

import { useAccounts, useDeleteAccount } from '../api';
import { AccountFormDialog } from '../components/AccountFormDialog';
import { AccountsTable } from '../components/AccountsTable';
import type { Account } from '../types';

export default function AccountsPage() {
  const [formOpen, setFormOpen] = useState(false);
  const [selected, setSelected] = useState<Account | undefined>(undefined);
  const [toDelete, setToDelete] = useState<Account | null>(null);

  const accountsQuery = useAccounts(true);
  const deleteAccount = useDeleteAccount();

  const sorted = useMemo(
    () =>
      [...(accountsQuery.data ?? [])].sort((a, b) => a.name.localeCompare(b.name)),
    [accountsQuery.data],
  );

  const handleCreate = () => {
    setSelected(undefined);
    setFormOpen(true);
  };

  const handleEdit = (account: Account) => {
    setSelected(account);
    setFormOpen(true);
  };

  const handleConfirmDelete = async () => {
    if (!toDelete) {
      return;
    }
    try {
      await deleteAccount.mutateAsync(toDelete.id);
      toast.success('Account deleted');
      setToDelete(null);
    } catch (error) {
      toast.error(error instanceof Error ? error.message : 'Failed to delete account');
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader
        title="Accounts"
        description="Where you keep your money: checking, savings, credit cards, cash."
        actions={
          <Button onClick={handleCreate}>
            <PlusIcon />
            New account
          </Button>
        }
      />

      <AsyncBoundary
        isPending={accountsQuery.isPending}
        error={accountsQuery.error}
        isEmpty={sorted.length === 0}
        emptyTitle="No accounts yet"
        emptyDescription="Create your first account to start tracking transactions."
        emptyAction={
          <Button onClick={handleCreate}>
            <PlusIcon />
            Create account
          </Button>
        }
        loadingFallback={
          <div className="space-y-2">
            <Skeleton className="h-10 w-full" />
            <Skeleton className="h-10 w-full" />
            <Skeleton className="h-10 w-full" />
          </div>
        }
      >
        <AccountsTable
          accounts={sorted}
          onEdit={handleEdit}
          onDelete={(account) => setToDelete(account)}
        />
      </AsyncBoundary>

      <AccountFormDialog
        open={formOpen}
        onOpenChange={(open) => {
          setFormOpen(open);
          if (!open) {
            setSelected(undefined);
          }
        }}
        account={selected}
      />

      <ConfirmDialog
        open={toDelete !== null}
        onOpenChange={(open) => {
          if (!open) {
            setToDelete(null);
          }
        }}
        title={toDelete ? `Delete "${toDelete.name}"?` : 'Delete account?'}
        description="This account and any associated transactions on the server will be affected. This cannot be undone."
        confirmLabel="Delete"
        destructive
        loading={deleteAccount.isPending}
        onConfirm={handleConfirmDelete}
      />
    </div>
  );
}
