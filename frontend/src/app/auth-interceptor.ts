import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('jwt');
  const expiration = localStorage.getItem('jwt_expiration');

  const router = inject(Router);

  if (token && expiration) {
    const currentTime = Math.floor(Date.now() / 1000); // Current time in seconds

    // If the token has expired
    if (Number(expiration) < currentTime) {
      // Remove the expired token and expiration time
      localStorage.removeItem('jwt');
      localStorage.removeItem('jwt_expiration');
      // Redirect the user to the login page
      router.navigate(['/login']);
      return throwError('Token has expired');
    }

    // Clone the request and add the Authorization header
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    // Continue with the request
    return next(clonedRequest);
  }

  // If no token is found, just continue with the request
  return next(req);
};
