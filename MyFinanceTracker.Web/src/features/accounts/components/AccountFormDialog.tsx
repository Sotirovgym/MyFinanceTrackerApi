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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Switch } from '@/components/ui/switch';

import { AccountType, AccountTypeOptions } from '@/shared/enums/accountType';
import { CurrencyCode, CurrencyCodeOptions } from '@/shared/enums/currencyCode';
import { mapServerErrors } from '@/shared/forms/mapServerErrors';

import { useCreateAccount, useUpdateAccount } from '../api';
import {
  createAccountSchema,
  updateAccountSchema,
  type CreateAccountFormValues,
  type UpdateAccountFormValues,
} from '../schemas';
import type { Account } from '../types';

type AccountFormDialogProps = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  account?: Account;
};

export function AccountFormDialog({ open, onOpenChange, account }: AccountFormDialogProps) {
  const isEdit = account !== undefined;
  return isEdit ? (
    <EditDialog open={open} onOpenChange={onOpenChange} account={account!} />
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
  const createAccount = useCreateAccount();

  const form = useForm<CreateAccountFormValues>({
    resolver: zodResolver(createAccountSchema),
    defaultValues: {
      name: '',
      accountType: AccountType.Checking,
      currency: CurrencyCode.USD,
      initialBalance: 0,
    },
  });

  useEffect(() => {
    if (open) {
      form.reset({
        name: '',
        accountType: AccountType.Checking,
        currency: CurrencyCode.USD,
        initialBalance: 0,
      });
    }
  }, [open, form]);

  const onSubmit = form.handleSubmit(async (values) => {
    try {
      await createAccount.mutateAsync(values);
      toast.success('Account created');
      onOpenChange(false);
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, {
        knownFields: ['name', 'accountType', 'currency', 'initialBalance'],
      });
      if (!applied) {
        toast.error(error instanceof Error ? error.message : 'Failed to create account');
      }
    }
  });

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>New account</DialogTitle>
          <DialogDescription>Add a checking, savings, credit card, or cash account.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            <NameField control={form.control} />
            <AccountTypeField control={form.control} />
            <CurrencyField control={form.control} />
            <FormField
              control={form.control}
              name="initialBalance"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Initial balance</FormLabel>
                  <FormControl>
                    <Input
                      type="number"
                      step="0.01"
                      inputMode="decimal"
                      value={Number.isFinite(field.value) ? field.value : ''}
                      onChange={(e) =>
                        field.onChange(e.target.value === '' ? Number.NaN : Number(e.target.value))
                      }
                      onBlur={field.onBlur}
                      name={field.name}
                      ref={field.ref}
                    />
                  </FormControl>
                  <FormDescription>The starting balance for this account.</FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />
            <DialogFooter>
              <Button type="button" variant="ghost" onClick={() => onOpenChange(false)}>
                Cancel
              </Button>
              <Button type="submit" disabled={createAccount.isPending}>
                {createAccount.isPending ? 'Creating...' : 'Create account'}
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
  account,
}: {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  account: Account;
}) {
  const updateAccount = useUpdateAccount();

  const form = useForm<UpdateAccountFormValues>({
    resolver: zodResolver(updateAccountSchema),
    defaultValues: {
      name: account.name,
      accountType: account.accountType,
      currency: account.currency,
      isActive: account.isActive,
    },
  });

  useEffect(() => {
    if (open) {
      form.reset({
        name: account.name,
        accountType: account.accountType,
        currency: account.currency,
        isActive: account.isActive,
      });
    }
  }, [open, account, form]);

  const onSubmit = form.handleSubmit(async (values) => {
    try {
      await updateAccount.mutateAsync({ id: account.id, request: values });
      toast.success('Account updated');
      onOpenChange(false);
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, {
        knownFields: ['name', 'accountType', 'currency', 'isActive'],
      });
      if (!applied) {
        toast.error(error instanceof Error ? error.message : 'Failed to update account');
      }
    }
  });

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit account</DialogTitle>
          <DialogDescription>Update this account&apos;s details.</DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            <NameField control={form.control} />
            <AccountTypeField control={form.control} />
            <CurrencyField control={form.control} />
            <FormField
              control={form.control}
              name="isActive"
              render={({ field }) => (
                <FormItem className="border-border flex flex-row items-center justify-between rounded-lg border p-3">
                  <div className="space-y-0.5">
                    <FormLabel>Active</FormLabel>
                    <FormDescription>Inactive accounts are hidden by default.</FormDescription>
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
              <Button type="submit" disabled={updateAccount.isPending}>
                {updateAccount.isPending ? 'Saving...' : 'Save changes'}
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}

type SharedFormControl =
  | ReturnType<typeof useForm<CreateAccountFormValues>>['control']
  | ReturnType<typeof useForm<UpdateAccountFormValues>>['control'];

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
            <Input placeholder="e.g. Main checking" {...field} />
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}

function AccountTypeField({ control }: { control: SharedFormControl }) {
  return (
    <FormField
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      control={control as any}
      name="accountType"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Account type</FormLabel>
          <Select
            value={String(field.value)}
            onValueChange={(v) => field.onChange(Number(v))}
          >
            <FormControl>
              <SelectTrigger className="w-full">
                <SelectValue placeholder="Select account type" />
              </SelectTrigger>
            </FormControl>
            <SelectContent>
              {AccountTypeOptions.map((option) => (
                <SelectItem key={option.value} value={String(option.value)}>
                  {option.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}

function CurrencyField({ control }: { control: SharedFormControl }) {
  return (
    <FormField
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      control={control as any}
      name="currency"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Currency</FormLabel>
          <Select
            value={String(field.value)}
            onValueChange={(v) => field.onChange(Number(v))}
          >
            <FormControl>
              <SelectTrigger className="w-full">
                <SelectValue placeholder="Select currency" />
              </SelectTrigger>
            </FormControl>
            <SelectContent>
              {CurrencyCodeOptions.map((option) => (
                <SelectItem key={option.value} value={String(option.value)}>
                  {option.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
