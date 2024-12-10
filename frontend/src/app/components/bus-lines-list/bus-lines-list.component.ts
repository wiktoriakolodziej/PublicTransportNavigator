import { Component, HostListener, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { Router } from '@angular/router';
import { BusService } from '../../services/bus/bus.service';
import { BusDTO } from '../../models/bus';

@Component({
  selector: 'app-bus-lines-list',
  standalone: true,
  imports: [MatGridListModule, MatButtonModule],
  templateUrl: './bus-lines-list.component.html',
  styleUrl: './bus-lines-list.component.scss'
})
export class BusLinesListComponent implements OnInit {
  columns: number = 4;
  tiles: number[] = [];

  constructor(private router: Router, private busService: BusService) { 
    this.adjustColumns(window.innerWidth);
  }
  ngOnInit(): void {
    this.busService.getBuses().subscribe({
      next: (buses : BusDTO[]) =>{
        buses.forEach(element => {
          this.tiles.push(element.number);
        })
      }
    });
  }

  onBusLine(number: number): void{
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


