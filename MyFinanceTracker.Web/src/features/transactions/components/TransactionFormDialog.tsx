import { useEffect, useMemo } from 'react';
import { useForm, useWatch } from 'react-hook-form';
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
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { Textarea } from '@/components/ui/textarea';

import { Combobox, type ComboboxOption } from '@/shared/forms/Combobox';
import { DatePicker } from '@/shared/forms/DatePicker';
import { mapServerErrors } from '@/shared/forms/mapServerErrors';
import { TransactionType, TransactionTypeLabels } from '@/shared/enums/transactionType';

import { useAccounts } from '@/features/accounts/api';
import { useCategories } from '@/features/categories/api';

import { useCreateTransaction, useUpdateTransaction } from '../api';
import { transactionSchema, type TransactionFormValues } from '../schemas';
import type { Transaction } from '../types';

type TransactionFormDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  transaction?: Transaction;
};

const KNOWN_FIELDS = [
  'accountId',
  'categoryId',
  'type',
  'amount',
  'transactionDate',
  'description',
] as const;

export function TransactionFormDialog({
  open,
  onOpenChange,
  transaction,
}: TransactionFormDialogProps) {
  const isEdit = transaction !== undefined;
  const createTransaction = useCreateTransaction();
  const updateTransaction = useUpdateTransaction();

  const accountsQuery = useAccounts(false);
  const categoriesQuery = useCategories(false);

  const accountOptions: ComboboxOption<string>[] = useMemo(
    () =>
      (accountsQuery.data ?? []).map((account) => ({
        value: account.id,
        label: account.name,
      })),
    [accountsQuery.data],
  );

  const form = useForm<TransactionFormValues>({
    resolver: zodResolver(transactionSchema),
    defaultValues: getDefaultValues(transaction),
  });

  const selectedType = useWatch({ control: form.control, name: 'type' });

  const categoryOptions: ComboboxOption<string>[] = useMemo(
    () =>
      (categoriesQuery.data ?? [])
        .filter((category) => category.type === selectedType)
        .map((category) => ({ value: category.id, label: category.name })),
    [categoriesQuery.data, selectedType],
  );

  useEffect(() => {
    if (open) {
      form.reset(getDefaultValues(transaction));
    }
  }, [open, transaction, form]);

  const onSubmit = form.handleSubmit(async (values) => {
    const trimmed = values.description.trim();
    const payload = {
      accountId: values.accountId,
      categoryId: values.categoryId,
      type: values.type,
      amount: values.amount,
      transactionDate: values.transactionDate.toISOString(),
      description: trimmed.length > 0 ? trimmed : null,
    };

    try {
      if (isEdit) {
        await updateTransaction.mutateAsync({ id: transaction.id, request: payload });
        toast.success('Transaction updated');
      } else {
        await createTransaction.mutateAsync(payload);
        toast.success('Transaction created');
      }
      onOpenChange(false);
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, {
        knownFields: [...KNOWN_FIELDS],
      });
      if (!applied) {
        toast.error(error instanceof Error ? error.message : 'Failed to save transaction');
      }
    }
  });

  const isPending = createTransaction.isPending || updateTransaction.isPending;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{isEdit ? 'Edit transaction' : 'New transaction'}</DialogTitle>
          <DialogDescription>
            Record income or an expense against one of your accounts.
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            <FormField
              control={form.control}
              name="type"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Type</FormLabel>
                  <FormControl>
                    <RadioGroup
                      className="grid grid-cols-2 gap-2"
                      value={String(field.value)}
                      onValueChange={(v) => {
                        const next = Number(v) as TransactionType;
                        field.onChange(next);
                        if (form.getValues('categoryId')) {
                          form.setValue('categoryId', '', { shouldValidate: false });
                        }
                      }}
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

            <FormField
              control={form.control}
              name="accountId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Account</FormLabel>
                  <FormControl>
                    <Combobox
                      value={field.value || undefined}
                      onChange={(value) => field.onChange(value ?? '')}
                      options={accountOptions}
                      placeholder="Pick an account"
                      searchPlaceholder="Search accounts..."
                      clearable={false}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="categoryId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Category</FormLabel>
                  <FormControl>
                    <Combobox
                      value={field.value || undefined}
                      onChange={(value) => field.onChange(value ?? '')}
                      options={categoryOptions}
                      placeholder={
                        categoryOptions.length === 0
                          ? `No ${TransactionTypeLabels[selectedType].toLowerCase()} categories`
                          : 'Pick a category'
                      }
                      searchPlaceholder="Search categories..."
                      clearable={false}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <div className="grid gap-4 sm:grid-cols-2">
              <FormField
                control={form.control}
                name="amount"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Amount</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        step="0.01"
                        min="0"
                        inputMode="decimal"
                        value={Number.isFinite(field.value) ? field.value : ''}
                        onChange={(e) =>
                          field.onChange(
                            e.target.value === '' ? Number.NaN : Number(e.target.value),
                          )
                        }
                        onBlur={field.onBlur}
                        name={field.name}
                        ref={field.ref}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="transactionDate"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Date</FormLabel>
                    <FormControl>
                      <DatePicker
                        value={field.value}
                        onChange={(value) => field.onChange(value ?? null)}
                        placeholder="Pick a date"
                        clearable={false}
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>

            <FormField
              control={form.control}
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description (optional)</FormLabel>
                  <FormControl>
                    <Textarea rows={2} {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <DialogFooter>
              <Button type="button" variant="ghost" onClick={() => onOpenChange(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={isPending}>
                {isPending ? 'Saving...' : isEdit ? 'Save changes' : 'Create transaction'}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}

function getDefaultValues(transaction?: Transaction): TransactionFormValues {
  if (transaction) {
    return {
      accountId: transaction.accountId,
      categoryId: transaction.categoryId,
      type: transaction.type,
      amount: transaction.amount,
      transactionDate: new Date(transaction.transactionDate),
      description: transaction.description ?? '',
    };
  }

  return {
    accountId: '',
    categoryId: '',
    type: TransactionType.Expense,
    amount: Number.NaN,
    transactionDate: new Date(),
    description: '',
  };
}
