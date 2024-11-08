import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-schedules-footer',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './schedules-footer.component.html',
  styleUrl: './schedules-footer.component.scss'
})
export class SchedulesFooterComponent {
  constructor(private router: Router) {}
  
  onBusLines() : void{
    this.router.navigate(['/schedulesBuses']);
  }
  onBusStops(): void{
    this.router.navigate(['/schedulesStops']);
  }
}
