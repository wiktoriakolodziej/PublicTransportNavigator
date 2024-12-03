import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { RoutePreviewDTO } from '../../models/route-preview';
import { Observable, of } from 'rxjs';
import { TimetableService } from '../timetable/timetable.service';
import { RouteDetailsDTO } from '../../models/route-details';
import { HistoryDetailsDTO } from '../../models/history-details';

@Injectable({
  providedIn: 'root'
})
export class UserTravelService {
  private baseUrl = apiUrl;
  private serviceUrl = "UserTravels"

  constructor(private http: HttpClient, private timetableService: TimetableService) { }


  getHistoryPreview(userId: number, page: number, pageSize: number) : Observable<RoutePreviewDTO[]>{
      return this.http.get<RoutePreviewDTO[]>(`${this.baseUrl}/${this.serviceUrl}/preview/${userId}/${page}/${pageSize}`);  
  }

  getHistoryDetails(id: number) : Observable<HistoryDetailsDTO>{
    return this.http.get<HistoryDetailsDTO>(`${this.baseUrl}/${this.serviceUrl}/${id}`);
  }
}

