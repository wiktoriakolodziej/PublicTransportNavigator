import { Injectable } from '@angular/core';
import { apiUrl } from '../../app.config';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BusTypeDTO } from '../../models/bus-type';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class BusTypeService {

  private baseUrl = apiUrl;
  private serviceUrl = 'BusTypes';

  constructor(private http: HttpClient) {}

  getByBusId(busId: number) : Observable<BusTypeDTO>{
    return this.http.get<BusTypeDTO>(`${this.baseUrl}/${this.serviceUrl}/ByBus/${busId}`);
  }


}
