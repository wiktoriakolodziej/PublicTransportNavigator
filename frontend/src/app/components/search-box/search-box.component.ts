import {Component, ElementRef, Input, ViewChild } from '@angular/core';
import {FormControl, FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatInputModule} from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BusStopService } from '../../services/busStop/bus-stop.service';
import { BusStopDTO } from '../../models/bus-stop';
import { LoginService } from '../../services/login/login.service';
import { UserInfo } from '../../models/user-info';
import { NgxTimepickerModule } from 'ngx-timepicker';
import { MatFormFieldModule } from '@angular/material/form-field';



@Component({
  selector: 'app-search-box',
  standalone: true,
  templateUrl: './search-box.component.html',
  styleUrls: ['./search-box.component.scss'],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatAutocompleteModule,  
    MatDatepickerModule,
    MatButtonModule, 
    MatCardModule,
    CommonModule,
    MatFormFieldModule,
  ],
  providers: [provideNativeDateAdapter()],
})
export class SearchBoxComponent{

  @Input() layoutType: 'horizontal' | 'vertical' = 'vertical';

  @ViewChild('inputFrom') inputFrom!: ElementRef<HTMLInputElement>;
  @ViewChild('inputTo') inputTo!: ElementRef<HTMLInputElement>;
  fromControl = new FormControl('');
  toControl = new FormControl('');
  filteredFromOptions: { [key: number]: string } = {};
  filteredFromValues : string[] = Object.values(this.filteredFromOptions);
  filteredToOptions: { [key: number]: string } = {};
  filteredToValues : string[] = Object.values(this.filteredToOptions);

  timeValue: string = '';
  formattedTime : string = '';


  isFirstFromFocus = true;
  isFirstToFocus = true;

  dateControl = new FormControl('');

  constructor(private router: Router, private busStopService: BusStopService, private loginService: LoginService) {}

  filter(type: string): void {
    
    switch(type){
      case "from":
        if(this.isFirstFromFocus && this.loginService.checkLoginStatus()){
          let user = JSON.parse(localStorage.getItem('userData')!) as UserInfo;
          this.busStopService.getFavouriteBusStops(user.id).subscribe(
            data => (
              data.forEach(element =>{
                this.filteredFromOptions[element.id] = element.name
                this.filteredFromValues.push(element.name);
              }
            )
          ));
          this.isFirstFromFocus = false;
          this.filteredFromValues = Object.values(this.filteredFromOptions);
        }
        else {
          let filterValue = this.inputFrom.nativeElement.value.toLowerCase();
          this.filteredFromOptions = [];
          this.filteredFromValues = [];
          if(filterValue.length == 0) break;
          this.busStopService.getNamesByFragment(filterValue).subscribe(
            next => (
              next.forEach(element =>{
                this.filteredFromOptions[element.id] = element.name
                this.filteredFromValues.push(element.name);
                }
              )
            )
          );
          this.filteredFromValues = Object.values(this.filteredFromOptions);
          
        }
        break;
      case "to":
        if(this.isFirstToFocus && this.loginService.checkLoginStatus()){
          let user = JSON.parse(localStorage.getItem('userData')!) as UserInfo;
          this.busStopService.getFavouriteBusStops(user.id).subscribe(
            data => (
              data.forEach(element =>{
               
                this.filteredToOptions[element.id] = element.name    
                this.filteredToValues.push(element.name); 
              }       
            )
          ));
          this.isFirstToFocus = false;
          this.filteredToValues = Object.values(this.filteredToOptions);
        }
        else {
          let filterValue = this.inputTo.nativeElement.value.toLowerCase();
          this.filteredToOptions = [];
          this.filteredToValues = [];
          if(filterValue.length == 0) break;
          this.busStopService.getNamesByFragment(filterValue).subscribe(
            next => (
              next.forEach(element =>{
                this.filteredToOptions[element.id] = element.name
                this.filteredToValues.push(element.name);
              }
                
              )
            )
          );
          
        }
        break;
        
    }
  }

  isFormComplete() {
    return this.dateControl.value && this.fromControl.value && this.toControl.value && this.formatTime(this.timeValue);
  }

  formatTime(time: string): boolean {
    const timeRegex = /^(\d{1,2}):(\d{1,2})$/;
    const match = this.timeValue.match(timeRegex);
  
    if (!match) {
      return false; 
    }
  
    let [_, hoursStr, minutesStr] = match;
    let hours = parseInt(hoursStr, 10);
    let minutes = parseInt(minutesStr, 10);
  

    if (hours < 0 || hours > 23 || minutes < 0 || minutes > 59) {
      return false;
    }
  
    const formattedHours = hours.toString().padStart(2, '0');
    const formattedMinutes = minutes.toString().padStart(2, '0');
  
    this.formattedTime = `${formattedHours}:${formattedMinutes}`;
    return true;
  }

  searchRoutes() {
    const dateValue = this.dateControl.value!;
  
    const date = new Date(dateValue); // Create a Date object
    const day = date.getDate(); // Get the day (1-31)
    const month = date.getMonth() + 1; // Get the month (0-11), so add 1 to get (1-12)
    const year = date.getFullYear(); // Get the year (YYYY)
    // Ensure month and day have two digits
    let formattedMonth = month.toString().padStart(2, '0');
    let formattedDay = day.toString().padStart(2, '0');

    // Format the date
    let formattedDate = `${year}-${formattedMonth}-${formattedDay}`;
    let dayOfWeek = date.getDay();
    console.log(dayOfWeek);
    dayOfWeek--;
    console.log(dayOfWeek);
    

    localStorage.setItem('selectedDate', formattedDate);
    localStorage.setItem('dayOfWeek', dayOfWeek.toString());

    const from = Object.entries(this.filteredFromOptions)
    .find(([key, value]) => value === this.fromControl.value)?.[0]; 
    const to =  Object.entries(this.filteredToOptions)
    .find(([key, value]) => value === this.toControl.value)?.[0]; 
    
    if(this.timeValue != this.formattedTime)
      this.formatTime(this.timeValue);
    


    this.router.navigate(['/mainPage']).then(() => {this.router.navigate(['/searchRoutes'], { 
      queryParams: {from: from, to: to,  date: formattedDate, time: this.formattedTime }
    });});
    
  }
}

