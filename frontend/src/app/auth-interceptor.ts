import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('jwt');
  const expiration = localStorage.getItem('jwt_expiration');

  const router = inject(Router);

  if (token && expiration) {
    const currentTime = Math.floor(Date.now() / 1000); 
    console.log('current time: ' + currentTime);
    console.log('expirtation' + Number(expiration));
    
    
    // If the token has expired
    if (Number(expiration) < currentTime) {
     
      localStorage.removeItem('jwt');
      localStorage.removeItem('jwt_expiration');
      localStorage.removeItem('userData')
    
      router.navigate(['/mainPage']);
      alert("You have been logged out, log in again");
      return throwError('Token has expired');
    }

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
