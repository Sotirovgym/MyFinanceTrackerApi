import { ApiError } from './ApiError';
import { isValidationProblemDetails, type FieldErrors } from './problemDetails';

export const AUTH_UNAUTHORIZED_EVENT = 'auth:unauthorized';

let accessToken: string | null = null;

export function setAccessToken(token: string | null): void {
  accessToken = token;
}

type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE';

type RequestOptions = {
  signal?: AbortSignal;
  query?: Record<string, string | number | boolean | undefined | null>;
};

function buildUrl(path: string, query?: RequestOptions['query']): string {
  const baseUrl = import.meta.env.VITE_API_BASE_URL ?? '';
  const url = `${baseUrl}${path}`;

  if (!query) {
    return url;
  }

  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(query)) {
    if (value === undefined || value === null || value === '') {
      continue;
    }
    params.append(key, String(value));
  }

  const queryString = params.toString();
  return queryString.length > 0 ? `${url}?${queryString}` : url;
}

async function parseError(response: Response): Promise<ApiError> {
  let message = response.statusText || `Request failed with status ${response.status}`;
  let fieldErrors: FieldErrors | undefined;

  try {
    const text = await response.text();
    if (text.length > 0) {
      try {
        const parsed: unknown = JSON.parse(text);
        if (isValidationProblemDetails(parsed)) {
          message = parsed.title ?? parsed.detail ?? message;
          fieldErrors = parsed.errors;
        } else if (typeof parsed === 'string') {
          message = parsed;
        }
      } catch {
        message = text;
      }
    }
  } catch {
    // Swallow body-read errors; keep the default status-text message.
  }

  return new ApiError(response.status, message, fieldErrors);
}

async function request<T>(
  method: HttpMethod,
  path: string,
  body?: unknown,
  options?: RequestOptions,
): Promise<T> {
  const headers: Record<string, string> = {
    Accept: 'application/json',
  };

  if (body !== undefined) {
    headers['Content-Type'] = 'application/json';
  }

  if (accessToken) {
    headers.Authorization = `Bearer ${accessToken}`;
  }

  const response = await fetch(buildUrl(path, options?.query), {
    method,
    headers,
    body: body === undefined ? undefined : JSON.stringify(body),
    signal: options?.signal,
  });

  if (response.status === 401) {
    accessToken = null;
    window.dispatchEvent(new Event(AUTH_UNAUTHORIZED_EVENT));
  }

  if (!response.ok) {
    throw await parseError(response);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const text = await response.text();
  if (text.length === 0) {
    return undefined as T;
  }

  return JSON.parse(text) as T;
}

export const api = {
  get: <T>(path: string, options?: RequestOptions) => request<T>('GET', path, undefined, options),
  post: <T>(path: string, body?: unknown, options?: RequestOptions) =>
    request<T>('POST', path, body, options),
  put: <T>(path: string, body?: unknown, options?: RequestOptions) =>
    request<T>('PUT', path, body, options),
  del: <T = void>(path: string, options?: RequestOptions) =>
    request<T>('DELETE', path, undefined, options),
};
