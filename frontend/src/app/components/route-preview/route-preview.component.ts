import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { RoutePreviewDTO } from '../../models/route-preview';

@Component({
  selector: 'app-route-preview',
  standalone: true,
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './route-preview.component.html',
  styleUrl: './route-preview.component.scss'
})
export class RoutePreviewComponent {
    @Input() copy!: RoutePreviewDTO;
    
    timeIn : string = "15:30";
    timeOut : string = "15:45";
    duration : string = "15 min";
    buses : string[] = [];

    ngOnChanges() {
      if (this.copy) { 
        const today = new Date();
        let dateTimeString = `${today.toISOString().split('T')[0]}T${this.copy.departureTime}:00`; // "YYYY-MM-DDTHH:mm:SS"
        let dateIn = new Date(dateTimeString);

        dateTimeString = `${today.toISOString().split('T')[0]}T${this.copy.destinationTime}:00`;
        let dateOut = new Date(dateTimeString);

        this.timeIn = this.copy.departureTime;
        this.timeOut = this.copy.destinationTime;
        this.duration = this.copy.travelTime + " min";
        this.buses = this.copy.busNumbers; 
      }
    
    }
}
