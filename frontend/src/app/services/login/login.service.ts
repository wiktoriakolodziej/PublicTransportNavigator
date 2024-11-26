import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { catchError, map, Observable, of } from 'rxjs';
import { LoginResponseDTO } from '../../models/login-response';
import { UserInfo } from '../../models/user-info';
import { RegisterDTO } from '../../models/register';
import { RegisterResponseDTO } from '../../models/register-response';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private baseUrl = apiUrl;
  private serviceUrl = "Auth";

  constructor(private http: HttpClient) { }

  login(loginData: {Login : string, Password: string}) :Observable<LoginResponseDTO>{
    return this.http.post<LoginResponseDTO>(`${this.baseUrl}/${this.serviceUrl}/login`, loginData);
  }

  checkLoginStatus() : boolean{
    const userData = localStorage.getItem('userData');
    return !!userData;
  }

  logout() {
    localStorage.removeItem('userData');
    localStorage.removeItem('jwt');
  }

  getUserInfo(): UserInfo | null{
    const userData = localStorage.getItem('userData');
    return userData ? JSON.parse(userData) as UserInfo : null;
  }

  register(register: RegisterDTO): Observable<RegisterResponseDTO>{
    console.log('I ma here');
    console.log(register);
    console.log(`${this.baseUrl}/${this.serviceUrl}/register`);
    
    
    return this.http.post<RegisterResponseDTO>(`${this.baseUrl}/${this.serviceUrl}/register`, register);
  }

}
