import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BusCreateDTO } from '../../models/bus-create-dto';
import { apiUrl } from '../../app.config';
import { Observable, of } from 'rxjs';
import { BusSeatDTO } from '../../models/bus-seat';
import { BusDTO } from '../../models/bus';

@Injectable({
  providedIn: 'root',
})
export class BusService {
  private baseUrl = apiUrl;
  private serviceUrl = 'Buses';

  constructor(private http: HttpClient) {}

  getBuses() : Observable<BusDTO[]> {
    console.log(`${this.baseUrl}/${this.serviceUrl}`);
    
    return this.http.get<BusDTO[]>(`${this.baseUrl}/${this.serviceUrl}`);
  }

  getBusById(id : number){
    return this.http.get(`${this.baseUrl}/${this.serviceUrl}/${id}`);
  }

  createBus(busCreateDto: BusCreateDTO) {
    return this.http.post(`${this.baseUrl}/${this.serviceUrl}`, busCreateDto);
  }

  deleteBus (id : number){
    return this.http.delete(`${this.baseUrl}/${this.serviceUrl}/${id}`)
  }

  getBusSeats(busId: number, timeIn: string, timeOut: string, date: string): Observable<BusSeatDTO[]>{
    const params = new HttpParams()
    .set('timeIn', timeIn)        
    .set('timeOut', timeOut)       
    .set('date', date);    
    return this.http.get<BusSeatDTO[]>(`${this.baseUrl}/${this.serviceUrl}/seat/${busId}`, {params});
  }

  
}

