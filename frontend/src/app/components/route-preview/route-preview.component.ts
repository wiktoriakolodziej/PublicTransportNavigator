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
    buses : string[] = ["M1", "M2", "123", "M8"];

    ngOnChanges() {
      console.log(this.copy);
      if (this.copy) { 
        const today = new Date();
        let dateTimeString = `${today.toISOString().split('T')[0]}T${this.copy.timeIn}:00`; // "YYYY-MM-DDTHH:mm:SS"
        let dateIn = new Date(dateTimeString);

        dateTimeString = `${today.toISOString().split('T')[0]}T${this.copy.timeOut}:00`;
        let dateOut = new Date(dateTimeString);

        this.timeIn = this.copy.timeIn;
        this.timeOut = this.copy.timeOut;
        this.duration = ((dateOut.getTime() - dateIn.getTime()) / (1000 * 60)).toString() + " min";
        this.buses = this.copy.buses.map(bus => bus.number); 
      }
    
    }
}
