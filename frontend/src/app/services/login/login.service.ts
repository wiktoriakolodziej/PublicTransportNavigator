import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { LoginResponseDTO } from '../../models/login-response';

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
    localStorage.removeItem('token');
  }

}
