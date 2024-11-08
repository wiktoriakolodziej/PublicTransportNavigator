import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HeaderComponent } from '../../components/header/header.component';

@Component({
  selector: 'app-bus-line-page',
  standalone: true,
  imports: [HeaderComponent],
  templateUrl: './bus-line-page.component.html',
  styleUrl: './bus-line-page.component.scss'
})
export class BusLinePageComponent implements OnInit{
  lineNumber!: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // Get the bus line number from the route parameter
    this.lineNumber = this.route.snapshot.paramMap.get('lineNumber')!;
    console.log('Bus Line Number:', this.lineNumber);
  }

}
