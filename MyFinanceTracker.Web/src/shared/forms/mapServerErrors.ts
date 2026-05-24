import type { FieldValues, Path, UseFormSetError } from 'react-hook-form';
import { ApiError } from '@/shared/api/ApiError';

function toCamelCase(value: string): string {
  if (value.length === 0) {
    return value;
  }
  return value.charAt(0).toLowerCase() + value.slice(1);
}

export function mapServerErrors<TFieldValues extends FieldValues>(
  setError: UseFormSetError<TFieldValues>,
  error: unknown,
  options?: { knownFields?: Array<Path<TFieldValues>> },
): boolean {
  if (!(error instanceof ApiError) || !error.fieldErrors) {
    return false;
  }

  let applied = false;
  for (const [serverField, messages] of Object.entries(error.fieldErrors)) {
    const message = messages?.[0];
    if (!message) {
      continue;
    }

    const candidate = toCamelCase(serverField) as Path<TFieldValues>;
    if (options?.knownFields && !options.knownFields.includes(candidate)) {
      setError('root.serverError' as Path<TFieldValues>, { type: 'server', message });
    } else {
      setError(candidate, { type: 'server', message });
    }
    applied = true;
  }

  return applied;
}
