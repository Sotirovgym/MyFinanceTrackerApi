import { format, parseISO } from 'date-fns';

export function formatDate(value: Date | string): string {
  const date = typeof value === 'string' ? parseISO(value) : value;
  return format(date, 'PP');
}

export function formatDateTime(value: Date | string): string {
  const date = typeof value === 'string' ? parseISO(value) : value;
  return format(date, 'PPp');
}

export function toIsoDateString(value: Date): string {
  return value.toISOString();
}

export function startOfDayUtc(value: Date): Date {
  return new Date(Date.UTC(value.getFullYear(), value.getMonth(), value.getDate(), 0, 0, 0, 0));
}

export function endOfDayUtc(value: Date): Date {
  return new Date(
    Date.UTC(value.getFullYear(), value.getMonth(), value.getDate(), 23, 59, 59, 999),
  );
}
