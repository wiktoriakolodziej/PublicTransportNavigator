import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = apiUrl;
  private serviceUrl = "User"

  constructor(private http: HttpClient) { }


}
