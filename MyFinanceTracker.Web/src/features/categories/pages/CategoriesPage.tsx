import { useMemo, useState } from 'react';
import { PlusIcon } from 'lucide-react';
import { toast } from 'sonner';

import { Button } from '@/components/ui/button';
import { Skeleton } from '@/components/ui/skeleton';

import { PageHeader } from '@/shared/components/PageHeader';
import { AsyncBoundary } from '@/shared/components/AsyncBoundary';
import { ConfirmDialog } from '@/shared/components/ConfirmDialog';

import { useCategories, useDeleteCategory } from '../api';
import { CategoryFormDialog } from '../components/CategoryFormDialog';
import { CategoriesTable } from '../components/CategoriesTable';
import type { Category } from '../types';

export default function CategoriesPage() {
  const [formOpen, setFormOpen] = useState(false);
  const [selected, setSelected] = useState<Category | undefined>(undefined);
  const [toDelete, setToDelete] = useState<Category | null>(null);

  const categoriesQuery = useCategories(true);
  const deleteCategory = useDeleteCategory();

  const sorted = useMemo(
    () =>
      [...(categoriesQuery.data ?? [])].sort((a, b) => {
        if (a.type !== b.type) {
          return a.type - b.type;
        }
        return a.name.localeCompare(b.name);
      }),
    [categoriesQuery.data],
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
      await deleteCategory.mutateAsync(toDelete.id);
      toast.success('Category deleted');
      setToDelete(null);
    } catch (error) {
      toast.error(error instanceof Error ? error.message : 'Failed to delete category');
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader
        title="Categories"
        description="Group your transactions so reports are meaningful."
        actions={
          <Button onClick={handleCreate}>
            <PlusIcon />
            New category
          </Button>
        }
      />

      <AsyncBoundary
        isPending={categoriesQuery.isPending}
        error={categoriesQuery.error}
        isEmpty={sorted.length === 0}
        emptyTitle="No categories yet"
        emptyDescription="Create a few categories like Groceries, Rent, or Salary to get started."
        emptyAction={
          <Button onClick={handleCreate}>
            <PlusIcon />
            Create category
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
        <CategoriesTable
          categories={sorted}
          onEdit={(category) => {
            setSelected(category);
            setFormOpen(true);
          }}
          onDelete={(category) => setToDelete(category)}
        />
      </AsyncBoundary>

      <CategoryFormDialog
        open={formOpen}
        onOpenChange={(open) => {
          setFormOpen(open);
          if (!open) {
            setSelected(undefined);
          }
        }}
        category={selected}
      />

      <ConfirmDialog
        open={toDelete !== null}
        onOpenChange={(open) => {
          if (!open) {
            setToDelete(null);
          }
        }}
        title={toDelete ? `Delete "${toDelete.name}"?` : 'Delete category?'}
        description="If transactions reference this category, the server may reject the delete."
        confirmLabel="Delete"
        destructive
        loading={deleteCategory.isPending}
        onConfirm={handleConfirmDelete}
      />
    </div>
  );
}
