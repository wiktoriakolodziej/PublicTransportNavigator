import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseUrl = apiUrl;

  constructor(private http: HttpClient) {}

  getImage(imagePath: string) : string{
    return `${this.baseUrl}${imagePath}`;
  }
}
