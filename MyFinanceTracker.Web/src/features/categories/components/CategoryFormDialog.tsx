import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { toast } from 'sonner';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { Switch } from '@/components/ui/switch';

import { TransactionType, TransactionTypeLabels } from '@/shared/enums/transactionType';
import { mapServerErrors } from '@/shared/forms/mapServerErrors';

import { useCreateCategory, useUpdateCategory } from '../api';
import {
  createCategorySchema,
  updateCategorySchema,
  type CreateCategoryFormValues,
  type UpdateCategoryFormValues,
} from '../schemas';
import type { Category } from '../types';

type CategoryFormDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  category?: Category;
};

export function CategoryFormDialog({ open, onOpenChange, category }: CategoryFormDialogProps) {
  return category ? (
    <EditDialog open={open} onOpenChange={onOpenChange} category={category} />
  ) : (
    <CreateDialog open={open} onOpenChange={onOpenChange} />
  );
}

function CreateDialog({
  open,
  onOpenChange,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
}) {
  const createCategory = useCreateCategory();

  const form = useForm<CreateCategoryFormValues>({
    resolver: zodResolver(createCategorySchema),
    defaultValues: { name: '', type: TransactionType.Expense },
  });

  useEffect(() => {
    if (open) {
      form.reset({ name: '', type: TransactionType.Expense });
    }
  }, [open, form]);

  const onSubmit = form.handleSubmit(async (values) => {
    try {
      await createCategory.mutateAsync(values);
      toast.success('Category created');
      onOpenChange(false);
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, { knownFields: ['name', 'type'] });
      if (!applied) {
        toast.error(error instanceof Error ? error.message : 'Failed to create category');
      }
    }
  });

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>New category</DialogTitle>
          <DialogDescription>Categories group your transactions for reporting.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            <NameField control={form.control} />
            <TypeField control={form.control} />
            <DialogFooter>
              <Button type="button" variant="ghost" onClick={() => onOpenChange(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={createCategory.isPending}>
                {createCategory.isPending ? 'Creating...' : 'Create category'}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}

function EditDialog({
  open,
  onOpenChange,
  category,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  category: Category;
}) {
  const updateCategory = useUpdateCategory();

  const form = useForm<UpdateCategoryFormValues>({
    resolver: zodResolver(updateCategorySchema),
    defaultValues: {
      name: category.name,
      type: category.type,
      isActive: category.isActive,
    },
  });

  useEffect(() => {
    if (open) {
      form.reset({
        name: category.name,
        type: category.type,
        isActive: category.isActive,
      });
    }
  }, [open, category, form]);

  const onSubmit = form.handleSubmit(async (values) => {
    try {
      await updateCategory.mutateAsync({ id: category.id, request: values });
      toast.success('Category updated');
      onOpenChange(false);
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, {
        knownFields: ['name', 'type', 'isActive'],
      });
      if (!applied) {
        toast.error(error instanceof Error ? error.message : 'Failed to update category');
      }
    }
  });

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit category</DialogTitle>
          <DialogDescription>Update this category&apos;s details.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            <NameField control={form.control} />
            <TypeField control={form.control} />
            <FormField
              control={form.control}
              name="isActive"
              render={({ field }) => (
                <FormItem className="border-border flex flex-row items-center justify-between rounded-lg border p-3">
                  <div className="space-y-0.5">
                    <FormLabel>Active</FormLabel>
                    <FormDescription>Inactive categories are hidden by default.</FormDescription>
                  </div>
                  <FormControl>
                    <Switch checked={field.value} onCheckedChange={field.onChange} />
                  </FormControl>
                </FormItem>
              )}
            />
            <DialogFooter>
              <Button type="button" variant="ghost" onClick={() => onOpenChange(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={updateCategory.isPending}>
                {updateCategory.isPending ? 'Saving...' : 'Save changes'}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}

type SharedFormControl =
  | ReturnType<typeof useForm<CreateCategoryFormValues>>['control']
  | ReturnType<typeof useForm<UpdateCategoryFormValues>>['control'];

function NameField({ control }: { control: SharedFormControl }) {
  return (
    <FormField
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      control={control as any}
      name="name"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Name</FormLabel>
          <FormControl>
            <Input placeholder="e.g. Groceries" {...field} />
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}

function TypeField({ control }: { control: SharedFormControl }) {
  return (
    <FormField
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      control={control as any}
      name="type"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Type</FormLabel>
          <FormControl>
            <RadioGroup
              className="grid grid-cols-2 gap-2"
              value={String(field.value)}
              onValueChange={(v) => field.onChange(Number(v))}
            >
              <label className="border-border has-data-[state=checked]:border-primary flex cursor-pointer items-center gap-2 rounded-lg border p-3 text-sm">
                <RadioGroupItem value={String(TransactionType.Income)} />
                {TransactionTypeLabels[TransactionType.Income]}
              </label>
              <label className="border-border has-data-[state=checked]:border-primary flex cursor-pointer items-center gap-2 rounded-lg border p-3 text-sm">
                <RadioGroupItem value={String(TransactionType.Expense)} />
                {TransactionTypeLabels[TransactionType.Expense]}
              </label>
            </RadioGroup>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
