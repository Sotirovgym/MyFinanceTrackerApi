const STORAGE_KEY = 'mft.auth';

export type StoredAuth = {
  accessToken: string;
  expiresAt: string;
};

export function readStoredAuth(): StoredAuth | null {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) {
      return null;
    }
    const parsed = JSON.parse(raw) as StoredAuth;
    if (!parsed.accessToken || !parsed.expiresAt) {
      return null;
    }
    return parsed;
  } catch {
    return null;
  }
}

export function writeStoredAuth(value: StoredAuth): void {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(value));
}

export function clearStoredAuth(): void {
  localStorage.removeItem(STORAGE_KEY);
}

export function isExpired(expiresAt: string): boolean {
  const ts = Date.parse(expiresAt);
  if (Number.isNaN(ts)) {
    return true;
  }
  return ts <= Date.now();
}
