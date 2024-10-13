import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  authService.isAuthentification.subscribe({
    next: (value) => {
      if (value) {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ${authService.getToken()}`,
          },
        });
      }
    },
  });

  return next(req).pipe(
    catchError((e: HttpErrorResponse) => {
      if (e.status === 401) {
        authService.logout();
        router.navigate(['']);
      }
      const error = e.error?.error?.message || e.statusText;
      return throwError(() => error);
    })
  );
};
