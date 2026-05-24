/* eslint-disable react-refresh/only-export-components */
import { createContext, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { AUTH_UNAUTHORIZED_EVENT, setAccessToken } from '@/shared/api/client';
import {
  clearStoredAuth,
  isExpired,
  readStoredAuth,
  writeStoredAuth,
  type StoredAuth,
} from './tokenStorage';

export type AuthSession = StoredAuth;

export type AuthContextValue = {
  session: AuthSession | null;
  isAuthenticated: boolean;
  signIn: (session: AuthSession) => void;
  signOut: () => void;
};

export const AuthContext = createContext<AuthContextValue | null>(null);

type AuthProviderProps = {
  children: React.ReactNode;
};

export function AuthProvider({ children }: AuthProviderProps) {
  const [session, setSession] = useState<AuthSession | null>(() => {
    const stored = readStoredAuth();
    if (!stored) {
      return null;
    }
    if (isExpired(stored.expiresAt)) {
      clearStoredAuth();
      return null;
    }
    setAccessToken(stored.accessToken);
    return stored;
  });

  const sessionRef = useRef(session);

  useEffect(() => {
    sessionRef.current = session;
  }, [session]);

  const signIn = useCallback((next: AuthSession) => {
    writeStoredAuth(next);
    setAccessToken(next.accessToken);
    setSession(next);
  }, []);

  const signOut = useCallback(() => {
    clearStoredAuth();
    setAccessToken(null);
    setSession(null);
  }, []);

  useEffect(() => {
    const handler = () => {
      if (sessionRef.current) {
        signOut();
      }
    };

    window.addEventListener(AUTH_UNAUTHORIZED_EVENT, handler);
    return () => window.removeEventListener(AUTH_UNAUTHORIZED_EVENT, handler);
  }, [signOut]);

  const value = useMemo<AuthContextValue>(
    () => ({
      session,
      isAuthenticated: session !== null,
      signIn,
      signOut,
    }),
    [session, signIn, signOut],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
