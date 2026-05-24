import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <main className="flex min-h-screen items-center justify-center p-8">
      <p className="text-muted-foreground text-sm">MyFinanceTracker — foundation setup complete.</p>
    </main>
  </StrictMode>,
);
