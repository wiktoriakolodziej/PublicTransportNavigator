import { Component, OnInit } from '@angular/core';
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
export class SearchPageComponent implements OnInit {

  showFiller = false;
  from: string = '';
  to: string = '';
  date: string = '';
  routes: RoutePreviewDTO[] = [];

  constructor(private route: ActivatedRoute, private service: TimetableService, private router: Router) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.from = params['from'];
      this.to = params['to'];
      this.date = params['date'];
    });
    this.getRoutes();
  }

  getRoutes() {
    this.service.getRoutes(this.from, this.to, this.date).subscribe({
      next: (data: RoutePreviewDTO[]) => {
        this.routes = data;
      },
      error: (err) => {
        console.error('Error fetching routes:', err);
      }
    });
  }

  routeDetails(busRoute: RoutePreviewDTO){
      this.router.navigate(['/routeDetails', {object: JSON.stringify(busRoute)}]);
  }
}
