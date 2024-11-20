import { AfterViewInit, Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { SearchBoxComponent } from '../../components/search-box/search-box.component';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutePreviewComponent } from '../../components/route-preview/route-preview.component';
import { RoutePreviewDTO } from '../../models/route-preview';
import { TimetableService } from '../../services/timetable/timetable.service';
import { MapComponent } from '../../components/map/map.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CoordinateDTO } from '../../models/coordinateDTO';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [HeaderComponent, 
            SearchBoxComponent,
            RoutePreviewComponent,
            MapComponent,
            MatSidenavModule,
            MatButtonModule,
            MatIconModule,],
  templateUrl: './search-page.component.html',
  styleUrl: './search-page.component.scss'
})
export class SearchPageComponent implements OnInit, AfterViewInit  {

  showFiller = false;
  from: number = -1;
  to: number = -1;
  time: string = '';
  date: string = '';
  routes: RoutePreviewDTO[] = [];

  higlightedRoute: CoordinateDTO[] = [];

  constructor(private route: ActivatedRoute, private service: TimetableService, private router: Router) {}

  ngAfterViewInit(): void {
    this.onRouteHover(this.routes[0]);
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.from = params['from'];
      this.to = params['to'];
      this.time = params['time'];
      this.date = params['date'];
    });
    this.getRoutes();
  }

  getRoutes() {
    this.service.getRoutes(this.from, this.to, this.time).subscribe({
      next: (data: RoutePreviewDTO) => {
        this.routes.push(data);
        this.higlightedRoute = data.coordinates;   
      },
      error: (err) => {
        console.error('Error fetching routes:', err);
      }
    });    
  }

  routeDetails(busRoute: RoutePreviewDTO){
      this.router.navigate(['/routeDetails', {routePreview: JSON.stringify(busRoute)}]);
  }

  onRouteHover(route: RoutePreviewDTO): void{
    this.higlightedRoute = route.coordinates;
  }


}
