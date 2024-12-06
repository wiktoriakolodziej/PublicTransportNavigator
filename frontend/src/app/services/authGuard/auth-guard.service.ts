import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate{

  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('jwt');
    console.log(token);
    
    if (token != null) {
      return true;
    } else {
      localStorage.removeItem('jwt');
      localStorage.removeItem('jwt_expiration');
      localStorage.removeItem('userData')
      this.router.navigate(['/mainPage']);
      alert("You have been logged out, log in again");
      return false;
    }
  }
}