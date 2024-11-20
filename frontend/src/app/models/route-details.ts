import { CoordinateDTO } from "./coordinateDTO";
import { PartDTO } from "./part";

export interface RouteDetailsDTO {
    id: string;
    departureTime: string;
    destinationTime: string;
    travelTime: number,
    parts: PartDTO[],
    coordinates: CoordinateDTO[]
  }