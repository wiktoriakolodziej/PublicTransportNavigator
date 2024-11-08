import { Injectable } from '@angular/core';
import { apiUrl, ETagName } from '../../app.config';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map, Observable, of, tap } from 'rxjs';
import { BusStopDTO } from '../../models/bus-stop';
import { BusStopDetailsDTO } from '../../models/bus-stop-details';
import { UserInfo } from '../../models/user-info';

@Injectable({
  providedIn: 'root'
})
export class BusStopService {

  private baseUrl = apiUrl;
  private serviceUrl = "BusStops";
  private eTag = ETagName;



  constructor(private http: HttpClient) {}

  getBusStopById(id : number) :Observable<BusStopDTO | null>{

    const etag: string = `etag_${id}`;
    const data: string = `busStop_${id}`;

    const headers = localStorage.getItem(etag) ? new HttpHeaders({ 'If-None-Match': localStorage.getItem(etag)! }) : undefined;

    return this.http.get<BusStopDTO>(`${this.baseUrl}/${this.serviceUrl}/${id}`, { headers, observe: 'response' }).pipe(
      tap(response => {
        if(response.status === 200){
          if(response.body) localStorage.setItem(data, JSON.stringify(response.body));
          const newETag = response.headers.get(this.eTag);
          if(newETag) localStorage.setItem(etag, newETag);
        }
      }),
      map(response => response.status === 304 ? JSON.parse(localStorage.getItem(data)!) as BusStopDTO  : response.body)
    );
  }

  getAll() : Observable<BusStopDTO[] | null>{

    // const etag: string = `busStopsETag`;
    // const data: string = `busStops`;
    // const headers = localStorage.getItem(etag) ? new HttpHeaders({ 'If-None-Match': localStorage.getItem(etag)! }) : undefined;

    // return this.http.get<BusStopDTO[]>(`${this.baseUrl}/${this.serviceUrl}`, { headers, observe: 'response' }).pipe(
    //   tap(response => {
    //     if (response.status === 200) { 
    //       localStorage.setItem(data, JSON.stringify(response.body));
    //       const newEtag = response.headers.get(this.eTag);
    //       if (newEtag) {
    //         localStorage.setItem(etag, newEtag); // Cache ETag
    //       }
    //     }
    //   }),
    //   map(response => response.status === 304 ? JSON.parse(localStorage.getItem(data)!) as BusStopDTO[] : response.body)
    // );

    let result : BusStopDTO[] = [
      {
        id: 1,
        name: "domi",
        coordX: 1,
        coordY: 1,
        onRequest: true,
      },
      {
        id: 2,
        name: "domi4",
        coordX: 1,
        coordY: 1,
        onRequest: true,
      },
      {
        id: 3,
        name: "domi3",
        coordX: 1,
        coordY: 1,
        onRequest: true,
      },
      {
        id: 4,
        name: "domi4",
        coordX: 1,
        coordY: 1,
        onRequest: true,
      }
    ]

    return of(result);
  }

  getTimetables(busStopId: number) : Observable<BusStopDetailsDTO | null>{

    // const etag: string = `timetableEtag_${busStopId}`;
    // const data: string = `timetableData_${busStopId}`;
    // const headers = localStorage.getItem(etag) ? new HttpHeaders({ 'If-None-Match': localStorage.getItem(etag)! }) : undefined;

    // return this.http.get<BusStopDetailsDTO>(`${this.baseUrl}/${this.serviceUrl}/timetables/${busStopId}`, { headers, observe: 'response' }).pipe(
    //   tap(response => {
    //     if(response.status === 200){
    //       if(response.body) localStorage.setItem(data, JSON.stringify(response.body));
    //       const newEtag = response.headers.get(this.eTag);
    //       if(newEtag) localStorage.setItem(etag, newEtag);
    //     }
    //   }),
    //   map(response => response.status === 304 ? JSON.parse(localStorage.getItem(data)!) as BusStopDetailsDTO : response.body)
    // );

    let result : BusStopDetailsDTO = {
      id: busStopId,
      name: "DOmi",
      onRequest: true,
      buses : [
          {
            id: 1,
            number: "M1",
            time: [
              "12:45",
              "13:45",
              "14:45"
            ]
          },
          {
            id: 2,
            number: "M100",
            time: [
              "1:45",
              "2:45",
              "3:45"
            ]
          },
          {
            id: 3,
            number: "A4",
            time: [
              "12:45",
              "12:55",
              "13:05",
              "13:15",
              "13:25",
              "13:35"
            ]
          }
      ]
    }
    return of(result);
  }

  getNamesByFragment(fragment: string) : Observable<BusStopDTO[]>{
    var user = JSON.parse(localStorage.getItem('userData')!) as UserInfo;
    let params = new HttpParams();

    if (user?.id) {
      params = params.set('userId', user.id);
    }
    var result =  this.http.get<BusStopDTO[]>(`${this.baseUrl}/${this.serviceUrl}/fragment/${fragment}`, {params});
    return result;
    
  }

  getFavouriteBusStops(userId: number) :Observable<BusStopDTO[]>{
    return this.http.get<BusStopDTO[]>(`${this.baseUrl}/${this.serviceUrl}/favourites/${userId}`);
  }

}

