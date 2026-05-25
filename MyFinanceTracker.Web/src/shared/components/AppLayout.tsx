import { useState } from 'react';
import { NavLink, Outlet } from 'react-router';
import {
  CoinsIcon,
  LayoutDashboardIcon,
  ListIcon,
  LogOutIcon,
  MenuIcon,
  TagsIcon,
  WalletIcon,
} from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { Sheet, SheetContent, SheetTitle, SheetTrigger } from '@/components/ui/sheet';
import { useAuth } from '@/shared/auth/useAuth';
import { ThemeToggle } from '@/shared/components/ThemeToggle';

const navItems = [
  { to: '/dashboard', label: 'Dashboard', icon: LayoutDashboardIcon },
  { to: '/accounts', label: 'Accounts', icon: WalletIcon },
  { to: '/categories', label: 'Categories', icon: TagsIcon },
  { to: '/transactions', label: 'Transactions', icon: ListIcon },
];

function NavLinks({ onNavigate }: { onNavigate?: () => void }) {
  return (
    <nav className="flex flex-col gap-1">
      {navItems.map(({ to, label, icon: Icon }) => (
        <NavLink
          key={to}
          to={to}
          onClick={onNavigate}
          className={({ isActive }) =>
            cn(
              'hover:bg-muted hover:text-foreground text-muted-foreground flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium transition-colors',
              isActive && 'bg-muted text-foreground',
            )
          }
        >
          <Icon className="size-4" />
          {label}
        </NavLink>
      ))}
    </nav>
  );
}

function Brand() {
  return (
    <div className="flex items-center gap-2 px-3 py-2">
      <CoinsIcon className="text-primary size-5" />
      <span className="text-sm font-semibold">MyFinanceTracker</span>
    </div>
  );
}

export function AppLayout() {
  const { signOut } = useAuth();
  const [mobileOpen, setMobileOpen] = useState(false);

  return (
    <div className="bg-background flex min-h-screen w-full">
      <aside className="bg-card border-border hidden w-64 shrink-0 flex-col border-r md:flex">
        <Brand />
        <Separator />
        <div className="flex-1 overflow-y-auto p-3">
          <NavLinks />
        </div>
        <Separator />
        <div className="p-3">
          <Button
            variant="ghost"
            className="w-full justify-start"
            onClick={() => signOut()}
          >
            <LogOutIcon className="mr-2 size-4" />
            Sign out
          </Button>
        </div>
      </aside>

      <div className="flex min-w-0 flex-1 flex-col">
        <header className="bg-background/95 supports-[backdrop-filter]:bg-background/70 border-border sticky top-0 z-10 flex h-14 items-center gap-2 border-b px-4 backdrop-blur md:px-6">
          <Sheet open={mobileOpen} onOpenChange={setMobileOpen}>
            <SheetTrigger asChild>
              <Button variant="ghost" size="icon" className="md:hidden" aria-label="Open menu">
                <MenuIcon />
              </Button>
            </SheetTrigger>
            <SheetContent side="left" className="w-72 p-0">
              <SheetTitle className="sr-only">Navigation</SheetTitle>
              <Brand />
              <Separator />
              <div className="p-3">
                <NavLinks onNavigate={() => setMobileOpen(false)} />
              </div>
            </SheetContent>
          </Sheet>
          <div className="ml-auto flex items-center gap-1">
            <ThemeToggle />
            <Button
              variant="ghost"
              size="icon"
              aria-label="Sign out"
              className="md:hidden"
              onClick={() => signOut()}
            >
              <LogOutIcon />
            </Button>
          </div>
        </header>
        <main className="mx-auto w-full max-w-6xl flex-1 px-4 py-6 md:px-6 md:py-8">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
