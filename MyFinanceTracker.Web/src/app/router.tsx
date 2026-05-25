/* eslint-disable react-refresh/only-export-components */
import { lazy, Suspense } from 'react';
import { createBrowserRouter, Navigate } from 'react-router';

import { AuthLayout } from '@/shared/components/AuthLayout';
import { AppLayout } from '@/shared/components/AppLayout';
import { ProtectedRoute } from '@/shared/auth/ProtectedRoute';
import { Skeleton } from '@/components/ui/skeleton';

const LoginPage = lazy(() => import('@/features/auth/pages/LoginPage'));
const RegisterPage = lazy(() => import('@/features/auth/pages/RegisterPage'));
const DashboardPage = lazy(() => import('@/features/dashboard/pages/DashboardPage'));
const AccountsPage = lazy(() => import('@/features/accounts/pages/AccountsPage'));
const CategoriesPage = lazy(() => import('@/features/categories/pages/CategoriesPage'));
const TransactionsPage = lazy(() => import('@/features/transactions/pages/TransactionsPage'));
const NotFoundPage = lazy(() => import('@/features/errors/NotFoundPage'));

function PageFallback() {
  return (
    <div className="space-y-3 p-2">
      <Skeleton className="h-8 w-48" />
      <Skeleton className="h-32 w-full" />
    </div>
  );
}

function withSuspense(node: React.ReactNode) {
  return <Suspense fallback={<PageFallback />}>{node}</Suspense>;
}

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Navigate to="/dashboard" replace />,
  },
  {
    element: <AuthLayout />,
    children: [
      { path: 'login', element: withSuspense(<LoginPage />) },
      { path: 'register', element: withSuspense(<RegisterPage />) },
    ],
  },
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          { path: 'dashboard', element: withSuspense(<DashboardPage />) },
          { path: 'accounts', element: withSuspense(<AccountsPage />) },
          { path: 'categories', element: withSuspense(<CategoriesPage />) },
          { path: 'transactions', element: withSuspense(<TransactionsPage />) },
        ],
      },
    ],
  },
  { path: '*', element: withSuspense(<NotFoundPage />) },
]);
