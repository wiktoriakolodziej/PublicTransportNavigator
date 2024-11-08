import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { RoutePreviewDTO } from '../../models/route-preview';
import { Observable, of } from 'rxjs';
import { RouteDetailsDTO } from '../../models/route-details';
import { BusStopDTO } from '../../models/bus-stop';

@Injectable({
  providedIn: 'root'
})
export class TimetableService {
  private baseUrl = apiUrl;

  constructor(private http: HttpClient) { }

  getRoutes(from: string, to: string, date: string): Observable<RoutePreviewDTO[]> {
    // return this.http.get<RoutePreviewDTO[]>(`${this.baseUrl}/timetable/routes`, {
    //   params: { from, to, date }
    // });
    const mockRoutes: RoutePreviewDTO[] = [
      {
        id: 1,
        timeIn: new Date('2024-10-16T08:30:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        timeOut: new Date('2024-10-16T09:00:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        buses: [
          { id: 1, number: 'Bus A' },
          { id: 2, number: 'Bus B' }
        ]
      },
      {
        id: 2,
        timeIn: new Date('2024-10-17T10:00:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        timeOut: new Date('2024-10-17T10:30:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        buses: [
          { id: 3, number: 'Bus C' },
          { id: 4, number: 'Bus D' }
        ]
      }
    ];

    // Return the mock data as an observable
    return of(mockRoutes);

  }

  getRouteDetails(id: number) : Observable<RouteDetailsDTO[]>{
    const mockRoutes: RouteDetailsDTO[] = [
      {
        id: 1,
        timetableIn: 
          {
            id: 1,
            busId: 1,
            busName: "Bus A",
            busStopId: 1,
            busStopName: "Domi1",
            time: new Date('2024-10-16T08:30:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
          },
          timetableOut: 
            {
              id: 2,
              busId: 1,
              busName: "Bus A",
              busStopId: 2,
              busStopName: "Domi2",
              time: new Date('2024-10-16T08:45:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
            },
          date: new Date('2024-10-16').toString(),
      },
      {
        id: 2,
        timetableIn: 
          {
            id: 3,
            busId: 2,
            busName: "Bus B",
            busStopId: 2,
            busStopName: "Domi2",
            time: new Date('2024-10-16T08:50:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
          },
          timetableOut: 
            {
              id: 4,
              busId: 2,
              busName: "Bus B",
              busStopId: 3,
              busStopName: "Domi3",
              time: new Date('2024-10-16T09:00:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
            },
            date: new Date('2024-10-16').toString(),
      },
      

      // {
      //   timeIn: new Date('2024-10-16T08:30:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      //   timeOut: new Date('2024-10-16T08:45:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      //   bus: { id: 1, number: 'Bus A' },
      //   busStopIn: {id: 1,
      //     name: "first BusStop",
      //     coordX: 12,
      //     coordY: 13,
      //     onRequest: true},
      //   busStopOut: {id: 1,
      //     name: "second BusStop",
      //     coordX: 12,
      //     coordY: 13,
      //     onRequest: true}
      // },
      // {
      //   id: 2,
      //   timeIn: new Date('2024-10-16T08:50:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      //   timeOut: new Date('2024-10-16T09:00:00').toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
      //   bus: { id: 1, number: 'Bus B' },
      //   busStopIn: {id: 2,
      //     name: "second BusStop",
      //     coordX: 12,
      //     coordY: 13,
      //     onRequest: true},
      //   busStopOut: {id: 3,
      //     name: "third BusStop",
      //     coordX: 12,
      //     coordY: 13,
      //     onRequest: true}
      // }
    ];
    return of(mockRoutes);
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
