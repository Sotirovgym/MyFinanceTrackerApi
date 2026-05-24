export type LoginRequest = {
  email: string;
  password: string;
};

export type RegisterRequest = {
  email: string;
  password: string;
  confirmPassword: string;
  enableNotifications: boolean;
};

export type LoginResponse = {
  accessToken: string;
  expiresAt: string;
};
