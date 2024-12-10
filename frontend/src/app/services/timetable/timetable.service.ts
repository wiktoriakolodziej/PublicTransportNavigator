import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { RoutePreviewDTO } from '../../models/route-preview';
import { Observable, of } from 'rxjs';
import { RouteDetailsDTO } from '../../models/route-details';
import { BusStopDTO } from '../../models/bus-stop';

@Injectable({
  providedIn: 'root'
})
export class TimetableService {
  private baseUrl = apiUrl;
  private serviceUrl = "Timetables";

  constructor(private http: HttpClient) { }

  getRoutes(from: number, to: number, time: string, dayOfWeek: number): Observable<RoutePreviewDTO> {
    console.log(dayOfWeek);
    
    const params = new HttpParams()
      .set('sourceBusStopId', from)        
      .set('destinationBusStopId', to)       
      .set('departureTime', time)
      .set('day', dayOfWeek);           

    return this.http.get<RoutePreviewDTO>(`${this.baseUrl}/${this.serviceUrl}/path`, { params });

  }

  getRouteDetails(id: string) : Observable<RouteDetailsDTO>{
    return this.http.get<RouteDetailsDTO>(`${this.baseUrl}/${this.serviceUrl}/path/details/${id}`);
  
  }

  getBusStops(busId: number, fromBusStopId: number, toBusStopId: number) : Observable<BusStopDTO[]>{
    const mockBusStops: BusStopDTO[] = [
      {
        id: 1,
        name: "DOmi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
      {
        id: 2,
        name: "Doomi",
        coordX: 12,
        coordY: 13,
        onRequest: true
      },
    ];
    return of(mockBusStops);
  }
}
