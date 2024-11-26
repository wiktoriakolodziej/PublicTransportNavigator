import { AfterViewChecked, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { SchedulesFooterComponent } from '../../components/schedules-footer/schedules-footer.component';
import { BusStopService } from '../../services/busStop/bus-stop.service';
import { BusStopDTO } from '../../models/bus-stop';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import {Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-schedules-stops-page',
  standalone: true,
  imports: [HeaderComponent, 
            SchedulesFooterComponent,
            MatGridListModule,
            MatCardModule,
            CommonModule,
  ],
  templateUrl: './schedules-stops-page.component.html',
  styleUrl: './schedules-stops-page.component.scss'
})
export class SchedulesStopsPageComponent implements OnInit, AfterViewChecked{
  @ViewChildren('contentElement') contentElements!: QueryList<ElementRef>;
  @ViewChild('ele') gridList!: ElementRef;
  busStops : BusStopDTO[] | null = null;
  columns: number = 2;

  constructor(private busStopsService: BusStopService, private router: Router){}
  ngOnInit(): void {
    this.busStopsService.getAll().subscribe({
      next: (data: BusStopDTO[] | null) => {
        console.log('bus stops ' + data);
        this.busStops = data;
      }
    });
  }

  ngAfterViewChecked() {   

    var test = this.gridList.nativeElement;
    test = 0.6 * test.clientWidth * 0.5;
    // Wait until the view is initialized
      this.contentElements.toArray().forEach(contentElement => {
        const contentWidth = contentElement.nativeElement.clientWidth;
        console.log(contentWidth);
       
        

        
        const isOverflowing = contentWidth > test;
        console.log(isOverflowing);
        

        // Apply 'scrolling-text' class if overflowing
        if (isOverflowing) {
          contentElement.nativeElement.classList.add('scrolling-text');
        } else {
          contentElement.nativeElement.classList.remove('scrolling-text');
        }
      });
  }

  goToBusStop(id: number){
    this.router.navigate(['/busStop', id]);
  }

  trackBusStopId(index: number, busStop: BusStopDTO): number {
    return busStop.id;  // Assuming busStop.id is unique for each busStop
  }


  @HostListener('window:resize', ['$event'])
  onResize(event: Event): void {
    this.ngAfterViewChecked();
  }


}
