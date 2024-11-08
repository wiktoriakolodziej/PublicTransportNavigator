import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { BusStopDTO } from '../../models/bus-stop';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-map',
  standalone: true,
  template: `<div id="map" style="height: inherit;"></div>`,
  styles: [],
  imports: [GoogleMapsModule],
})
export class MapComponent implements AfterViewInit  {
  private map: google.maps.Map | null = null;
  private directionsRenderer: google.maps.DirectionsRenderer | null = null;

  private busStops: BusStopDTO[] = [
    { id: 2, name: 'M1', coordX: 50.2877, coordY: 18.6767, onRequest: false },
    //{ id: 1, name: 'M1', coordX: 50.295472, coordY: 18.66894, onRequest: false },
    //{ id: 1, name: 'M1', coordX: 50.295472, coordY: 18.66894, onRequest: false },
    { id: 3, name: 'M1', coordX: 50.2984, coordY:  18.6776, onRequest: false },
    // Add more bus stops as needed
  ];

  constructor(private router: Router) {}

  ngAfterViewInit() {
    setTimeout(() => {
      this.initializeMap();
    }, 0);

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      if (!this.map) {
        this.initializeMap();  // Only initialize the map if it hasn't been initialized yet
      }
    });
  }

  private initializeMap() {
    const mapContainer = document.getElementById('map');
    if (mapContainer) {
      this.map = new google.maps.Map(mapContainer as HTMLElement, {
        center: { lat: this.busStops[0].coordX!, lng: this.busStops[0].coordY! }, // Center map on the first bus stop
        zoom: 14,
      });
      console.log("Map initialized");

      // Initialize directions renderer
      this.directionsRenderer = new google.maps.DirectionsRenderer({
        map: this.map,
        suppressMarkers: true, // Suppress default markers
        polylineOptions: {
          strokeColor: '#FF0000', // Color of the route
          strokeOpacity: 1.0,
          strokeWeight: 2,
        },
      });

      this.drawRoute();
    }
  }

  private drawRoute() {
    if (!this.map || !this.directionsRenderer) return;

    const directionsService = new google.maps.DirectionsService();

    // Set the start and end points
    const start = new google.maps.LatLng(this.busStops[0].coordX!, this.busStops[0].coordY!);
    const end = new google.maps.LatLng(this.busStops[this.busStops.length - 1].coordX!, this.busStops[this.busStops.length - 1].coordY!);

    // Define the request for directions
    const request = {
      origin: start,
      destination: end,
      travelMode: google.maps.TravelMode.TRANSIT, // Travel mode
    };

    // Get the directions
    directionsService.route(request, (result, status) => {
      if (status === google.maps.DirectionsStatus.OK) {
        this.directionsRenderer?.setDirections(result);
      } else {
        console.error('Directions request failed due to ' + status);
      }
    });
  }

}
