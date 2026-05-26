import * as React from 'react';
import { ChevronsUpDownIcon, XIcon } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';

export type ComboboxOption<TValue extends string | number> = {
  value: TValue;
  label: string;
  searchValue?: string;
};

type ComboboxProps<TValue extends string | number> = {
  value: TValue | undefined;
  onChange: (value: TValue | undefined) => void;
  options: ComboboxOption<TValue>[];
  placeholder?: string;
  searchPlaceholder?: string;
  emptyMessage?: string;
  disabled?: boolean;
  clearable?: boolean;
  className?: string;
  id?: string;
};

function ComboboxInner<TValue extends string | number>(
  {
    value,
    onChange,
    options,
    placeholder = 'Select...',
    searchPlaceholder = 'Search...',
    emptyMessage = 'No results.',
    disabled,
    clearable = true,
    className,
    id,
  }: ComboboxProps<TValue>,
  ref: React.ForwardedRef<HTMLButtonElement>,
) {
  const [open, setOpen] = React.useState(false);

  const selected = options.find((option) => option.value === value);

  const handleClear = (event: React.MouseEvent) => {
    event.preventDefault();
    event.stopPropagation();
    onChange(undefined);
  };

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          ref={ref}
          id={id}
          type="button"
          variant="outline"
          role="combobox"
          aria-expanded={open}
          disabled={disabled}
          className={cn(
            'w-full justify-between font-normal',
            !selected && 'text-muted-foreground',
            className,
          )}
        >
          <span className="flex-1 truncate text-left">
            {selected ? selected.label : placeholder}
          </span>
          {clearable && selected && !disabled ? (
            <span
              role="button"
              aria-label="Clear selection"
              onClick={handleClear}
              className="text-muted-foreground hover:text-foreground -mr-1 ml-2 inline-flex size-5 items-center justify-center rounded-sm"
            >
              <XIcon className="size-3.5" />
            </span>
          ) : (
            <ChevronsUpDownIcon className="text-muted-foreground ml-2 size-4 opacity-70" />
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent
        className="w-(--radix-popover-trigger-width) p-0"
        align="start"
      >
        <Command>
          <CommandInput placeholder={searchPlaceholder} />
          <CommandList>
            <CommandEmpty>{emptyMessage}</CommandEmpty>
            <CommandGroup>
              {options.map((option) => (
                <CommandItem
                  key={String(option.value)}
                  value={option.searchValue ?? option.label}
                  data-checked={option.value === value}
                  onSelect={() => {
                    onChange(option.value === value ? undefined : option.value);
                    setOpen(false);
                  }}
                >
                  {option.label}
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}

export const Combobox = React.forwardRef(ComboboxInner) as <TValue extends string | number>(
  props: ComboboxProps<TValue> & { ref?: React.ForwardedRef<HTMLButtonElement> },
) => React.ReactElement;
