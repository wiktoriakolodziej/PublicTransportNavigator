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
  private serviceUrl = "ReservedSeat"


  constructor(private http: HttpClient) { }

  createReservedSeat(seat : ReservdSeatCreateDTO) :Observable<ReservedSeatDTO>{
    return this.http.post<ReservedSeatDTO>(`${this.baseUrl}/${this.serviceUrl}`, seat);
  }

  confirmReservation(travelId: number): Observable<boolean>{
    return this.http.get<boolean>(`${this.baseUrl}/${this.serviceUrl}/confirm/${travelId}`, {observe: 'response'})
      .pipe(
        map(response => {
          return response.status === 200;
        }),
        catchError(() => {
          return of(false);
        })
      );
  }
}
