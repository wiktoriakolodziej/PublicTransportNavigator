import { BusDTO } from "./bus";

export interface RoutePreviewDTO {
    id: number;
    timeIn: string;
    timeOut: string;
    buses: BusDTO[]; 
  }