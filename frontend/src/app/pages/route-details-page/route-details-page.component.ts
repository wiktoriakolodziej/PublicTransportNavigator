import { Component, Input, input, OnInit } from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { MatDrawer, MatDrawerContainer, MatSidenavModule } from '@angular/material/sidenav';
import {MatIconModule } from '@angular/material/icon';
import { MapComponent } from '../../components/map/map.component';
import { RoutePreviewDTO } from '../../models/route-preview';
import { ActivatedRoute } from '@angular/router';
import { RoutePreviewComponent } from '../../components/route-preview/route-preview.component';
import { RouteDetailsDTO } from '../../models/route-details';
import { TimetableService } from '../../services/timetable/timetable.service';
import { MatButtonModule } from '@angular/material/button';
import { RouteDetailsComponent } from '../../components/route-details/route-details.component';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';
import { BusSeatDTO } from '../../models/bus-seat';
import { BusService } from '../../services/bus/bus.service';
import { ReservdSeatCreateDTO } from '../../models/reserved-seat-create';
import { ReservedSeatService } from '../../services/reservedSeat/reserved-seat.service';
import { ReservedSeatDTO } from '../../models/reserved-seat';
import { LoginService } from '../../services/login/login.service';

@Component({
  selector: 'app-route-details-page',
  standalone: true,
  imports: [HeaderComponent, 
            MatDrawerContainer, 
            MatDrawer, 
            MatIconModule,
            MatButtonModule, 
            MapComponent, 
            RoutePreviewComponent,
            MatSidenavModule,
            RouteDetailsComponent,
            MatCardModule,
            CommonModule ],
  templateUrl: './route-details-page.component.html',
  styleUrl: './route-details-page.component.scss'
})
export class RouteDetailsPageComponent implements OnInit {
    busRoute!: RoutePreviewDTO;
    routeDetails!: RouteDetailsDTO[];
    isReserveSeats: boolean = false;
    seats: BusSeatDTO[]= [];
    reservingForBus: number = 0;
    busImage?:string;
    chosenSeat: BusSeatDTO | null = null;
    nextOrFinishButtonLabel?: string;
    canReserveSeat: boolean = true;

    travelId? : number;

    constructor(private route: ActivatedRoute, private timetableService: TimetableService, private busService: BusService,
        private reservedSeatService: ReservedSeatService, private loginService : LoginService){}
  ngOnInit(): void {
    this.route.params.subscribe(params =>{
      this.busRoute = JSON.parse(params['object']);
    }); 
    this.timetableService.getRouteDetails(this.busRoute.id).subscribe({
      next: (data: RouteDetailsDTO[]) => {
        this.routeDetails = data;
      },
      error: (err) => {
        console.error('Error fetching routes:', err);
      }
    });
    console.log(this.routeDetails);
    this.routeDetails = this.routeDetails.concat(this.routeDetails);
    this.nextOrFinishButtonLabel = this.routeDetails.length > 1 ?  "next seat" : "confirm";
  }

  reserveSeats(): void{
      this.isReserveSeats = true;
      this.canReserveSeat = false;
      this.busService.getBusSeats(this.routeDetails[this.reservingForBus].timetableIn.busId).subscribe({
        next: (data: BusSeatDTO[]) =>{
          this.seats = data;
        }
      })
  }

  isSeatReserved(seat: any): boolean {
    return seat.reserved;   
  }
  isSeatChosen(seat: any): boolean {
    return seat == this.chosenSeat;   
  }

  reserveSeat(seat: any) {
    if (!seat.reserved) {
      this.chosenSeat = seat;
    } else {
      alert('This seat is already reserved.');
    }
  }

  nextSeat(): boolean{
    if(this.chosenSeat == null)
    {
      alert('pick up a seat first');
      return false;
    }
    //create reserved seat
    let item: ReservdSeatCreateDTO = {
      busSeatId: this.chosenSeat.id,
      timetableInId: this.routeDetails[this.reservingForBus].timetableIn.id,
      timetableOffId: this.routeDetails[this.reservingForBus].timetableOut.id,
      reservationDate: this.routeDetails[this.reservingForBus].date
    }
    this.reservedSeatService.createReservedSeat(item).subscribe({
      next: (data: ReservedSeatDTO) =>{
        if(this.travelId == null){
          this.travelId = data.travelId;
        }
      }
    })

    this.reservingForBus++;
    if(this.routeDetails.length == this.reservingForBus + 1){
      this.nextOrFinishButtonLabel = "confirm";
    }
    this.busService.getBusSeats(this.routeDetails[this.reservingForBus].timetableIn.busId).subscribe({
      next: (data: BusSeatDTO[]) =>{
        this.seats = data;
      }
    });
    this.chosenSeat = null;
    return true;
  }

  confirm(): void{
    if(this.chosenSeat == null)
      {
        alert('pick up a seat first');
        return;
      }
    //create reserved seat
    let item: ReservdSeatCreateDTO = {
      busSeatId: this.chosenSeat.id,
      timetableInId: this.routeDetails[this.reservingForBus].timetableIn.id,
      timetableOffId: this.routeDetails[this.reservingForBus].timetableOut.id,
      reservationDate: this.routeDetails[this.reservingForBus].date
    }
    this.reservedSeatService.createReservedSeat(item);
    if(!this.reservedSeatService.confirmReservation(this.travelId!)){
      alert('try again');
    }
    this.isReserveSeats = false;
    alert('you have succesfully booked a trip');

  }

  checkIfLoggedIn() : boolean{
    return this.loginService.checkLoginStatus();
  }
}
