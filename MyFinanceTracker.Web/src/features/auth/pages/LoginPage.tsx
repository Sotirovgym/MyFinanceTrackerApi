import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Link, useLocation, useNavigate } from 'react-router';
import { toast } from 'sonner';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Alert, AlertDescription } from '@/components/ui/alert';

import { useAuth } from '@/shared/auth/useAuth';
import { mapServerErrors } from '@/shared/forms/mapServerErrors';
import { useLogin } from '@/features/auth/api';
import { loginSchema, type LoginFormValues } from '@/features/auth/schemas';

type LocationState = { from?: string } | null;

export default function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();
  const { signIn } = useAuth();
  const login = useLogin();

  const form = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: { email: '', password: '' },
  });

  const onSubmit = form.handleSubmit(async (values) => {
    form.clearErrors('root');

    try {
      const response = await login.mutateAsync(values);
      signIn(response);
      toast.success('Welcome back!');
      const redirectTo = (location.state as LocationState)?.from ?? '/dashboard';
      navigate(redirectTo, { replace: true });
    } catch (error) {
      const applied = mapServerErrors(form.setError, error, {
        knownFields: ['email', 'password'],
      });
      if (!applied) {
        const message = error instanceof Error ? error.message : 'Login failed.';
        form.setError('root.serverError', { type: 'server', message });
      }
    }
  });

  const rootError = form.formState.errors.root?.serverError?.message;

  return (
    <Card>
      <CardHeader>
        <CardTitle>Sign in</CardTitle>
        <CardDescription>Welcome back. Enter your credentials to continue.</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={onSubmit} className="space-y-4" noValidate>
            {rootError ? (
              <Alert variant="destructive">
                <AlertDescription>{rootError}</AlertDescription>
              </Alert>
            ) : null}

            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input type="email" autoComplete="email" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Password</FormLabel>
                  <FormControl>
                    <Input type="password" autoComplete="current-password" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <Button type="submit" className="w-full" disabled={login.isPending}>
              {login.isPending ? 'Signing in...' : 'Sign in'}
            </Button>

            <p className="text-muted-foreground text-center text-sm">
              Don&apos;t have an account?{' '}
              <Link to="/register" className="text-foreground font-medium hover:underline">
                Create one
              </Link>
            </p>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
