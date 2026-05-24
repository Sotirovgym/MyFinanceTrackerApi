export type FieldErrors = Record<string, string[]>;

export type ValidationProblemDetails = {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  errors?: FieldErrors;
};

export function isValidationProblemDetails(value: unknown): value is ValidationProblemDetails {
  if (value === null || typeof value !== 'object') {
    return false;
  }

  const candidate = value as Record<string, unknown>;
  return 'errors' in candidate || 'title' in candidate || 'status' in candidate;
}
