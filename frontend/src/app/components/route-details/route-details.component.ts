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
    return Object.entries(this.part.details).map(([time, busStopName]) => `${time} : ${busStopName}`);
  }
  toggleDisplay(){
    this.shouldShowBusStops = !this.shouldShowBusStops;
  }
  getDetailsById(index: number = -1): [string, string] | undefined {
    if (index === -1) {
      index = Object.keys(this.part.details).length - 1;
    }
    const entries = Object.entries(this.part.details);
    if (index >= 0 && index < entries.length) {
      const [time, busStopName] = entries[index];
      return [time, busStopName]; 
    }
    return undefined; // Return undefined if the index is out of bounds
  }
  // getBusStops() : void{
  //   if(this.busStops == null){
  //     this.timetableService.getBusStops(this.routeDetailsDTO.timetableIn.busId, this.routeDetailsDTO.timetableOut.busStopId, this.routeDetailsDTO.timetableIn.busStopId).subscribe({
  //       next: (data: BusStopDTO[]) => {
  //         this.busStops = data;
  //       },
  //       error: (err) => {
  //         console.error('Error fetching bus stops:', err);
  //       }
  //     });
  //   }
  //   this.shouldShowBusStops = !this.shouldShowBusStops;
  //   console.log(this.busStops);
  // }

}
