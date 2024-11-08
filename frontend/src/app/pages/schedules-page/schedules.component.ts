import {Component} from '@angular/core';
import { HeaderComponent } from '../../components/header/header.component';
import { BusLinesListComponent } from '../../components/bus-lines-list/bus-lines-list.component';
import { SchedulesFooterComponent } from '../../components/schedules-footer/schedules-footer.component';

@Component({
  selector: 'app-schedules',
  standalone: true,
  imports: [HeaderComponent, BusLinesListComponent, SchedulesFooterComponent],
  templateUrl: './schedules.component.html',
  styleUrl: './schedules.component.scss'
})
export class SchedulesPageComponent { 


}
