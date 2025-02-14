import { Component, OnInit } from '@angular/core';
import {MatCardModule } from '@angular/material/card';
import { UserInfo } from '../../models/user-info';
import { Router } from '@angular/router';
import { MatChipsModule } from '@angular/material/chips';
import { HeaderComponent } from '../../components/header/header.component';
import { RoutePreviewDTO } from '../../models/route-preview';
import { UserTravelService } from '../../services/userTravel/user-travel.service';
import { RoutePreviewComponent } from '../../components/route-preview/route-preview.component';

@Component({
  selector: 'app-account-page',
  standalone: true,
  imports: [MatCardModule,
            MatChipsModule,
            HeaderComponent,
            RoutePreviewComponent],
  templateUrl: './account-page.component.html',
  styleUrl: './account-page.component.scss'
})
export class AccountPageComponent implements OnInit{
    user? : UserInfo;
    history? : RoutePreviewDTO[];
    page: number = 1;
    pageSize : number = 2;

    constructor(private router: Router, private travelService: UserTravelService){
    }
  ngOnInit(): void {
    const userData = localStorage.getItem('userData');
      if (userData) {
        this.user = JSON.parse(userData) as UserInfo;
      }
      
    this.travelService.getHistoryPreview(this.user!.id, this.page, this.pageSize).subscribe({
      next: (data: RoutePreviewDTO[]) => {
        this.history = data;
      }});
      
  }

    showInfo(info: string){
      alert(info);
    }


    switchPage(where : number){
      this.page += where;
      if(this.page < 1) this.page =1;
      this.travelService.getHistoryPreview(this.user!.id, this.page, this.pageSize).subscribe({
        next: (data: RoutePreviewDTO[]) => {
          this.history = data;
        }});
    }


}
