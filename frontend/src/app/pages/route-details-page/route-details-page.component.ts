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
import { UserInfo } from '../../models/user-info';

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
      this.getSeatDetails();   
  }

  isSeatReserved(seat: BusSeatDTO): boolean {
    return !seat.available;   
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

    this.createSeat();

    this.reservingForBus++;
    if(this.routeDetails.parts.length == this.reservingForBus + 1){
      this.nextOrFinishButtonLabel = "confirm";
    }

    this.getSeatDetails();
    return true;
  }

  getSeatDetails(){
    var busId = this.routeDetails.parts[this.reservingForBus].busId;
    const times = Object.keys(this.routeDetails.parts[this.reservingForBus].details);
    var timeIn = times[0];
    var timeOut = times[times.length - 1];
    var date = localStorage.getItem('selectedDate');
    this.busService.getBusSeats(busId, timeIn, timeOut, date!).subscribe(busSeat =>{
        console.log(busSeat);
        this.seats = busSeat;      
      }
    );
    this.busTypeService.getByBusId(this.routeDetails.parts[this.reservingForBus].busId).subscribe(path => {
      let relativePath = path.imagePath;
      this.busImagePath = this.getImage(relativePath);
    });
  }

  confirm(): void{
    if(this.chosenSeat == null)
    {
      alert('pick up a seat first');
      return;
    }
    
    this.createSeat();
    
    if(this.reservedSeatService.confirmReservation(this.travelId!).subscribe(result => { result > 0 })){
      this.isReserveSeats = false;
      alert('you have succesfully booked a trip');
    }
  }

  checkIfLoggedIn() : boolean{
    return this.loginService.checkLoginStatus();
  }

  getImage(path: string) : string{
    return this.imagService.getImage(path);
  }

  createSeat(): void{
  
      let part = this.routeDetails.parts[this.reservingForBus];
   
      let item: ReservdSeatCreateDTO = {
        busSeatId: this.chosenSeat!.id,
        timeIn: Object.keys(part.details)[0],
        timeOff: Object.keys(part.details)[Object.keys(part.details).length - 1],
        date: localStorage.getItem("selectedDate")!,
        userTravelId: this.travelId,
        userId: this.getUserData()!.id
      }
  
      this.reservedSeatService.createReservedSeat(item).subscribe({
        next: (data: ReservedSeatDTO) =>{
          
          if(this.travelId == null){
            this.travelId = data.userTravelId;
          }
        }
      });
  }

  getUserData(): UserInfo | null{
    return this.loginService.getUserInfo();
  }
}
