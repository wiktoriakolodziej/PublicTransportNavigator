import { BusDTO } from "./bus";
import { CoordinateDTO } from "./coordinateDTO";

export interface RoutePreviewDTO {
    id: string;
    departureTime: string;
    destinationTime: string;
    travelTime: number;
    busNumbers: string[];
    coordinates: CoordinateDTO[]; 
  }