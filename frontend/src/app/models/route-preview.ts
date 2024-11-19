import { BusDTO } from "./bus";
import { CoordinateDTO } from "./coordinateDTO";

export interface RoutePreviewDTO {
    id: number;
    departureTime: string;
    destinationTime: string;
    travelTime: number;
    busNumbers: number[];
    coordinates: CoordinateDTO[]; 
  }