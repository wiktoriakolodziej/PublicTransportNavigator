import { Component, HostListener } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { Router } from '@angular/router';

@Component({
  selector: 'app-bus-lines-list',
  standalone: true,
  imports: [MatGridListModule, MatButtonModule],
  templateUrl: './bus-lines-list.component.html',
  styleUrl: './bus-lines-list.component.scss'
})
export class BusLinesListComponent {
  columns: number = 4;
  tiles: string[] = ['1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6',
                      '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6',
                      '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6', '1', '2', '3', '4', '5', '6'];

  constructor(private router: Router) { 
    this.adjustColumns(window.innerWidth);
  }

  onBusLine(number: string): void{
    this.router.navigate(['/busLine', number]);
  }

    @HostListener('window:resize', ['$event'])
    onResize(event: any): void {
      this.adjustColumns(event.target.innerWidth);
    }

    adjustColumns(width: number): void {
      if (width > 1300) {
        this.columns = 15; // Full screen
      } else if (width > 800) {
        this.columns = 8; // Half screen
      } else {
        this.columns = 4; // Small screens
      }
    }
}
