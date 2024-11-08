import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BusStopDetailsDTO } from '../../models/bus-stop-details';
import { BusStopService } from '../../services/busStop/bus-stop.service';
import { HeaderComponent } from '../../components/header/header.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-bus-stop-page',
  standalone: true,
  imports: [HeaderComponent,
            MatCardModule
  ],
  templateUrl: './bus-stop-page.component.html',
  styleUrl: './bus-stop-page.component.scss'
})
export class BusStopPageComponent implements OnInit{
  busStopDetails: BusStopDetailsDTO | null = null;

  constructor(private route: ActivatedRoute, private busStopService: BusStopService) { }

  ngOnInit(): void {
    // Get the bus line number from the route parameter
    let id = this.route.snapshot.paramMap.get('stopNumber')!;
    this.busStopService.getTimetables(parseInt(id, 10)).subscribe({
      next: (data: BusStopDetailsDTO | null) => {
        this.busStopDetails = data;
        this.busStopDetails!.buses = this.busStopDetails!.buses.concat(this.busStopDetails!.buses);
        this.busStopDetails!.buses = this.busStopDetails!.buses.concat(this.busStopDetails!.buses);
        this.busStopDetails!.buses = this.busStopDetails!.buses.concat(this.busStopDetails!.buses);
        this.busStopDetails!.buses = this.busStopDetails!.buses.concat(this.busStopDetails!.buses);
        this.busStopDetails!.buses = this.busStopDetails!.buses.concat(this.busStopDetails!.buses);
      }
    });
  }
}
