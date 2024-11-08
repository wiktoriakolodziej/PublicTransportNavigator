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
  private serviceUrl = "Login";
  private isLoggedInSubject = new BehaviorSubject<boolean>(false); 
  isLoggedIn$: Observable<boolean> = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(loginData: {username : string, password: string}) :Observable<LoginResponseDTO>{
    //return this.http.post(`${this.baseUrl}/${this.serviceUrl}`, loginData);
    let response: LoginResponseDTO = {
      token: "kfjskdfs",
      user: {
        id: 1,
        name: "domi",
        surname: "domi",
        favouriteBusStops: [
          {
            id: 1,
            name: "busstop1",
            userName: "home",
          },
          {
            id: 2,
            name: "busstop2",
            userName: "school",
          }
        ],
        discounts: [
          {
            id: 1,
            description: "only for students below 26 years old",
            name: "student",
            percent: 50,
          },
          {
            id: 2,
            description: "for travelers before 1pm",
            name: "early bird",
            percent: 10,
          }
        ]
      },
      expirationTime: 30000,
    }
    localStorage.setItem('userData', JSON.stringify(response.user)); 
    this.isLoggedInSubject.next(true);
    return of(response);
  }

  checkLoginStatus() : boolean{
    const userData = localStorage.getItem('userData');
    return !!userData;
  }

  logout() {
    this.isLoggedInSubject.next(false);
    localStorage.removeItem('userData');
  }

}
