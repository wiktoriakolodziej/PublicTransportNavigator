import { Component } from '@angular/core';
import { SearchBoxComponent } from '../../components/search-box/search-box.component';
import { HeaderComponent } from '../../components/header/header.component';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [SearchBoxComponent, HeaderComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
