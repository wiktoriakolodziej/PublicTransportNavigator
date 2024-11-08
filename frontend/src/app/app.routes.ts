import { Routes } from '@angular/router';
import { SchedulesPageComponent } from './pages/schedules-page/schedules.component'
import { MainPageComponent } from './pages/main-page/main-page.component';
import { BusLinePageComponent } from './pages/bus-line-page/bus-line-page.component';
import { SchedulesStopsPageComponent } from './pages/schedules-stops-page/schedules-stops-page.component';
import { SearchPageComponent } from './pages/search-page/search-page.component';
import { RouteDetailsPageComponent } from './pages/route-details-page/route-details-page.component';
import { AccountPageComponent } from './pages/account-page/account-page.component';
import { AuthGuardService } from './services/authGuard/auth-guard.service';
import { BusStopPageComponent } from './pages/bus-stop-page/bus-stop-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/mainPage', pathMatch: 'full' },
    { path: 'schedulesBuses', component: SchedulesPageComponent }, 
    { path: 'mainPage', component: MainPageComponent }, 
    { path: 'busLine/:lineNumber', component: BusLinePageComponent }, 
    { path: 'schedulesStops', component: SchedulesStopsPageComponent }, 
    { path: 'searchRoutes', component: SearchPageComponent }, 
    { path: 'routeDetails', component: RouteDetailsPageComponent }, 
    {
        path: 'account',
        component: AccountPageComponent,
        canActivate: [AuthGuardService],
    },
    {path: 'busStop/:stopNumber', component: BusStopPageComponent},
    
];
