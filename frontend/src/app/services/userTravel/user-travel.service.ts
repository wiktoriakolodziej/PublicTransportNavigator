import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { UserTravelCreateDTO } from '../../models/user-travel-create';
import { RoutePreviewDTO } from '../../models/route-preview';
import { Observable } from 'rxjs';
import { TimetableService } from '../timetable/timetable.service';

@Injectable({
  providedIn: 'root'
})
export class UserTravelService {
  private baseUrl = apiUrl;
  private serviceUrl = "UserTravel"

  constructor(private http: HttpClient, private timetableService: TimetableService) { }

  createReservedSeat(item : UserTravelCreateDTO){
    return this.http.post(`${this.baseUrl}/${this.serviceUrl}`, item);
  }

  getHistoryPreview(userId: number) : Observable<RoutePreviewDTO[]>{
      return this.timetableService.getRoutes("domi", "domi", "now");
  }
}

