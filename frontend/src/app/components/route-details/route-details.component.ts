import { Component, Input } from '@angular/core';
import { RouteDetailsDTO } from '../../models/route-details';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BusStopDTO } from '../../models/bus-stop';
import { TimetableService } from '../../services/timetable/timetable.service';
import { CommonModule } from '@angular/common';
import { BusStopService } from '../../services/busStop/bus-stop.service';

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
  @Input() routeDetailsDTO!: RouteDetailsDTO;
  busStops?: BusStopDTO[];
  shouldShowBusStops: boolean = false;

  constructor(private timetableService: TimetableService, private busStopService: BusStopService){}

  getBusStops() : void{
    if(this.busStops == null){
      this.timetableService.getBusStops(this.routeDetailsDTO.timetableIn.busId, this.routeDetailsDTO.timetableOut.busStopId, this.routeDetailsDTO.timetableIn.busStopId).subscribe({
        next: (data: BusStopDTO[]) => {
          this.busStops = data;
        },
        error: (err) => {
          console.error('Error fetching bus stops:', err);
        }
      });
    }
    this.shouldShowBusStops = !this.shouldShowBusStops;
    console.log(this.busStops);
  }

}
