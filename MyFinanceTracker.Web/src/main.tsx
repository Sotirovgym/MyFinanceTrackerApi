import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import { AppProviders } from '@/app/AppProviders';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AppProviders>
      <main className="flex min-h-screen items-center justify-center p-8">
        <p className="text-muted-foreground text-sm">MyFinanceTracker — shared plumbing ready.</p>
      </main>
    </AppProviders>
  </StrictMode>,
);
