import { AfterViewInit, Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';
import { BusStopDTO } from '../../models/bus-stop';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { CoordinateDTO } from '../../models/coordinateDTO';

@Component({
  selector: 'app-map',
  standalone: true,
  template: `<div id="map" style="height: inherit;"></div>`,
  styles: [],
  imports: [GoogleMapsModule],
})
export class MapComponent implements AfterViewInit, OnChanges  {
  private map: google.maps.Map | null = null;
  private markers: google.maps.Marker[] = []; // Keep track of markers

  @Input() coordinates!: CoordinateDTO[];

  constructor(private router: Router) {}
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['coordinates'] && this.map) {
      this.updateMarkers();
      console.log("update coords:" + this.coordinates); 
    }
  }

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
    if (!mapContainer) {
      console.error('Map container not found');
      return;
    }

    // Initialize the map
    this.map = new google.maps.Map(mapContainer, {
      center: { lat: 0, lng: 0 }, // Default center
      zoom: 8,                    // Default zoom level
    });

    // Add initial markers if coordinates are provided
    if (this.coordinates && this.coordinates.length > 0) {
      this.updateMarkers();
    }
  }

  private updateMarkers() {
    // Clear existing markers
    this.markers.forEach(marker => marker.setMap(null));
    this.markers = [];

    if (this.coordinates && this.coordinates.length > 0) {
      // Add new markers for the provided coordinates
      this.coordinates.forEach(coord => {
        const marker = new google.maps.Marker({
          position: { lat: coord.x, lng: coord.y },
          map: this.map!,
        });
        this.markers.push(marker);
      });

      // Adjust the map's bounds to fit all markers
      const bounds = new google.maps.LatLngBounds();
      this.coordinates.forEach(coord => bounds.extend({ lat: coord.x, lng: coord.y }));
      this.map!.fitBounds(bounds);
    }
  }

}
