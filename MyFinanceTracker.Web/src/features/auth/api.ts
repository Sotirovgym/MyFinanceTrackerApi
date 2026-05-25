import { useMutation } from '@tanstack/react-query';
import { api } from '@/shared/api/client';
import type { LoginRequest, LoginResponse, RegisterRequest } from './types';

export function useLogin() {
  return useMutation({
    mutationFn: (request: LoginRequest) => api.post<LoginResponse>('/api/Auth/login', request),
  });
}

export function useRegister() {
  return useMutation({
    mutationFn: (request: RegisterRequest) => api.post<string>('/api/Auth/register', request),
  });
}
