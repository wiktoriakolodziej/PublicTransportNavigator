import { Component, Input } from '@angular/core';
import { RouteDetailsDTO } from '../../models/route-details';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BusStopDTO } from '../../models/bus-stop';
import { TimetableService } from '../../services/timetable/timetable.service';
import { CommonModule } from '@angular/common';
import { BusStopService } from '../../services/busStop/bus-stop.service';
import { PartDTO } from '../../models/part';

@Component({
  selector: 'app-route-details',
  standalone: true,
  imports: [MatCardModule,
            MatButtonModule,
            MatIconModule,
            CommonModule,
  ],
  templateUrl: './route-details.component.html',
  styleUrl: './route-details.component.scss'
})
export class RouteDetailsComponent{
  @Input() part!: PartDTO;

  shouldShowBusStops: boolean = false;

  constructor(private timetableService: TimetableService, private busStopService: BusStopService){}

  getDetails() : string[]{
    return this.part.details.slice(1, this.part.details.length - 1);
  }
  toggleDisplay(){
    this.shouldShowBusStops = !this.shouldShowBusStops;
  }
  getDetailsById(index: number = -1): string | undefined {
    if (index === -1) {
      index = this.part.details.length - 1;
    }
    if(index < 0 || index >= this.part.details.length)
      return undefined; // Return undefined if the index is out of bounds

    return this.part.details[index]; 
  }

  shouldShowArrow(): boolean{
    return this.part.details.length > 2
  }
}
