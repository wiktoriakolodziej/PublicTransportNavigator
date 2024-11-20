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
import { BusTypeService } from '../../services/busType/bus-type.service';
import { SafeHtml, SafeUrl } from '@angular/platform-browser';
import { ImageService } from '../../services/image/image.service';

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
    routePreview!: RoutePreviewDTO;
    routeDetails!: RouteDetailsDTO;

    isReserveSeats: boolean = false;
    seats: BusSeatDTO[]= [];
    reservingForBus: number = 0;
    busImagePath?: string;
    chosenSeat: BusSeatDTO | null = null;
    nextOrFinishButtonLabel?: string;
    canReserveSeat: boolean = true;

    travelId? : number;

    constructor(private route: ActivatedRoute, private timetableService: TimetableService, private busService: BusService,
        private reservedSeatService: ReservedSeatService, private busTypeService: BusTypeService, private loginService : LoginService, private imagService: ImageService){}
  ngOnInit(): void {
    this.route.params.subscribe(params =>{
      this.routePreview = JSON.parse(params['routePreview']);
    }); 
    console.log(this.routePreview);
    
    this.timetableService.getRouteDetails(this.routePreview.id).subscribe({
      next: (data: RouteDetailsDTO) => {
        this.routeDetails = data;
        this.nextOrFinishButtonLabel = data.parts.length > 1 ?  "next seat" : "confirm";       
      },
      error: (err) => {
        console.error('Error fetching routes:', err);
      }
    });
  }

  reserveSeats(): void{
      this.isReserveSeats = true;
      this.canReserveSeat = false;
      this.busService.getBusSeats(this.routeDetails.parts[this.reservingForBus].busId).subscribe({
        next: (data: BusSeatDTO[]) =>{
          this.seats = data;
        }
      });
      this.busTypeService.getByBusId(this.routeDetails.parts[this.reservingForBus].busId).subscribe(path => {
        let relativePath = path.imagePath;
        this.busImagePath = this.imagService.getImage(relativePath);
      });
      
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
    let busData = this.routeDetails.parts[this.reservingForBus];
 
    let item: ReservdSeatCreateDTO = {
      busSeatId: this.chosenSeat.id,
      busIdIn: busData.busId,
      timeIn: Object.keys(busData.details)[0],
      timeOff: Object.keys(busData.details)[Object.keys(busData.details).length - 1],
      reservationDate: localStorage.getItem("selectedDate")!,

    }
    this.reservedSeatService.createReservedSeat(item).subscribe({
      next: (data: ReservedSeatDTO) =>{
        if(this.travelId == null){
          this.travelId = data.travelId;
        }
      }
    })

    this.reservingForBus++;
    if(this.routeDetails.parts.length == this.reservingForBus + 1){
      this.nextOrFinishButtonLabel = "confirm";
    }
    this.busService.getBusSeats(busData.busId).subscribe({
      next: (data: BusSeatDTO[]) =>{
        this.seats = data;
      }
    });
    this.chosenSeat = null;
    this.busTypeService.getByBusId(this.routeDetails.parts[this.reservingForBus].busId).subscribe(path => {
      let relativePath = path.imagePath;
      this.busImagePath = this.imagService.getImage(relativePath);
    });
    return true;
  }

  confirm(): void{
    if(this.chosenSeat == null)
      {
        alert('pick up a seat first');
        return;
      }
      let busData = this.routeDetails.parts[this.reservingForBus];
    //create reserved seat
    let item: ReservdSeatCreateDTO = {
      busSeatId: this.chosenSeat.id,
      busIdIn: busData.busId,
      timeIn: Object.keys(busData.details)[0],
      timeOff: Object.keys(busData.details)[Object.keys(busData.details).length - 1],
      reservationDate: localStorage.getItem("selectedDate")!,
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
