import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { SchedulesFooterComponent } from '../../components/schedules-footer/schedules-footer.component';
import { BusStopService } from '../../services/busStop/bus-stop.service';
import { BusStopDTO } from '../../models/bus-stop';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import {Router } from '@angular/router';

@Component({
  selector: 'app-schedules-stops-page',
  standalone: true,
  imports: [HeaderComponent, 
            SchedulesFooterComponent,
            MatGridListModule,
            MatCardModule,
  ],
  templateUrl: './schedules-stops-page.component.html',
  styleUrl: './schedules-stops-page.component.scss'
})
export class SchedulesStopsPageComponent implements OnInit{
  busStops : BusStopDTO[] | null = null;
  columns: number = 2;

  constructor(private busStopsService: BusStopService, private router: Router){}
  ngOnInit(): void {
    this.busStopsService.getAll().subscribe({
      next: (data: BusStopDTO[] | null) => {
        this.busStops = data;
      }
    })

    this.busStops = this.busStops!.concat(this.busStops!);
    this.busStops = this.busStops!.concat(this.busStops!);
    this.busStops = this.busStops!.concat(this.busStops!);
    this.busStops = this.busStops!.concat(this.busStops!);
    this.busStops = this.busStops!.concat(this.busStops!);
    this.busStops = this.busStops!.concat(this.busStops!);
  }

  goToBusStop(id: number){
    this.router.navigate(['/busStop', id]);
  }

}
