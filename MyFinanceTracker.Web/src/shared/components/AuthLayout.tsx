import { Outlet } from 'react-router';
import { CoinsIcon } from 'lucide-react';

export function AuthLayout() {
  return (
    <div className="bg-muted/40 min-h-screen w-full">
      <div className="mx-auto flex min-h-screen max-w-md flex-col justify-center px-4 py-12">
        <div className="mb-8 flex items-center justify-center gap-2">
          <CoinsIcon className="text-primary size-6" />
          <span className="text-lg font-semibold">MyFinanceTracker</span>
        </div>
        <Outlet />
      </div>
    </div>
  );
}
