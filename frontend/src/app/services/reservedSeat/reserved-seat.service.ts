import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { ReservdSeatCreateDTO } from '../../models/reserved-seat-create';
import { catchError, map, Observable, of } from 'rxjs';
import { ReservedSeatDTO } from '../../models/reserved-seat';

@Injectable({
  providedIn: 'root'
})
export class ReservedSeatService {
  private baseUrl = apiUrl;
  private serviceUrl = "ReservedSeats"


  constructor(private http: HttpClient) { }

  createReservedSeat(seat : ReservdSeatCreateDTO) :Observable<ReservedSeatDTO>{    
    return this.http.post<ReservedSeatDTO>(`${this.baseUrl}/${this.serviceUrl}`, seat);
  }

  confirmReservation(travelId: number): Observable<number>{
    return this.http.get<number>(`${this.baseUrl}/${this.serviceUrl}/confirm/${travelId}`); 
  }

}
