import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BusCreateDTO } from '../../models/bus-create-dto';
import { apiUrl } from '../../app.config';
import { Observable, of } from 'rxjs';
import { BusSeatDTO } from '../../models/bus-seat';

@Injectable({
  providedIn: 'root',
})
export class BusService {
  private baseUrl = apiUrl;

  constructor(private http: HttpClient) {}

  getBuses() {
    return this.http.get(`${this.baseUrl}/Buses`);
  }

  getBusById(id : number){
    return this.http.get(`${this.baseUrl}/Buses/${id}`);
  }

  createBus(busCreateDto: BusCreateDTO) {
    return this.http.post(`${this.baseUrl}/Buses`, busCreateDto);
  }

  deleteBus (id : number){
    return this.http.delete(`${this.baseUrl}/Buses/${id}`)
  }

  getBusSeats(busId: number): Observable<BusSeatDTO[]>{
    let busSeats: BusSeatDTO[] = [
      { id: 1, label: '1A', reserved: false, coordX: 14, coordY: 31 },
      { id: 2, label: '1B', reserved: false, coordX: 30, coordY: 31 },
      { id: 3, label: '2A', reserved: true, coordX: 14, coordY: 37 }, // Example reserved seat
      { id: 4, label: '2B', reserved: false, coordX: 30, coordY: 37 }
  ];
  return of(busSeats);
  }
}

