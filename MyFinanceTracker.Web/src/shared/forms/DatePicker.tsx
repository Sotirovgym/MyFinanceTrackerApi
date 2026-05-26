import * as React from 'react';
import { format } from 'date-fns';
import { CalendarIcon, XIcon } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';

type DatePickerProps = {
  value: Date | undefined;
  onChange: (value: Date | undefined) => void;
  placeholder?: string;
  disabled?: boolean;
  clearable?: boolean;
  className?: string;
  id?: string;
};

export const DatePicker = React.forwardRef<HTMLButtonElement, DatePickerProps>(
  function DatePicker(
    {
      value,
      onChange,
      placeholder = 'Pick a date',
      disabled,
      clearable = true,
      className,
      id,
    },
    ref,
  ) {
    const [open, setOpen] = React.useState(false);

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
            disabled={disabled}
            className={cn(
              'w-full justify-start text-left font-normal',
              !value && 'text-muted-foreground',
              className,
            )}
          >
            <CalendarIcon className="mr-2 size-4" />
            <span className="flex-1 truncate">
              {value ? format(value, 'PP') : placeholder}
            </span>
            {clearable && value && !disabled ? (
              <span
                role="button"
                aria-label="Clear date"
                onClick={handleClear}
                className="text-muted-foreground hover:text-foreground -mr-1 ml-2 inline-flex size-5 items-center justify-center rounded-sm"
              >
                <XIcon className="size-3.5" />
              </span>
            ) : null}
          </Button>
        </PopoverTrigger>
        <PopoverContent align="start" className="w-auto p-0">
          <Calendar
            mode="single"
            selected={value}
            onSelect={(next) => {
              onChange(next);
              setOpen(false);
            }}
            autoFocus
          />
        </PopoverContent>
      </Popover>
    );
  },
);
