import type { ReactNode } from 'react';
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert';
import { Skeleton } from '@/components/ui/skeleton';

type AsyncBoundaryProps = {
  isPending: boolean;
  error: unknown;
  isEmpty?: boolean;
  emptyTitle?: string;
  emptyDescription?: string;
  emptyAction?: ReactNode;
  loadingFallback?: ReactNode;
  children: ReactNode;
};

function defaultLoading() {
  return (
    <div className="space-y-2">
      <Skeleton className="h-9 w-full" />
      <Skeleton className="h-9 w-full" />
      <Skeleton className="h-9 w-full" />
    </div>
  );
}

function getErrorMessage(error: unknown): string {
  if (error instanceof Error) {
    return error.message;
  }
  if (typeof error === 'string') {
    return error;
  }
  return 'Something went wrong.';
}

export function AsyncBoundary({
  isPending,
  error,
  isEmpty,
  emptyTitle,
  emptyDescription,
  emptyAction,
  loadingFallback,
  children,
}: AsyncBoundaryProps) {
  if (isPending) {
    return <>{loadingFallback ?? defaultLoading()}</>;
  }

  if (error) {
    return (
      <Alert variant="destructive">
        <AlertTitle>Failed to load</AlertTitle>
        <AlertDescription>{getErrorMessage(error)}</AlertDescription>
      </Alert>
    );
  }

  if (isEmpty) {
    return (
      <div className="border-border bg-card flex flex-col items-center justify-center gap-3 rounded-lg border p-12 text-center">
        <h3 className="text-base font-semibold">{emptyTitle ?? 'Nothing here yet'}</h3>
        {emptyDescription ? (
          <p className="text-muted-foreground max-w-sm text-sm">{emptyDescription}</p>
        ) : null}
        {emptyAction ? <div className="pt-2">{emptyAction}</div> : null}
      </div>
    );
  }

  return <>{children}</>;
}
