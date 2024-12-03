import { CoordinateDTO } from "./coordinateDTO";
import { PartDTO } from "./part";

export interface HistoryDetailsDTO {
    id: string;
    departureTime: string;
    destinationTime: string;
    travelTime: number,
    DepartureStopName : string
    DestinationStopName : string
    coordinates: CoordinateDTO[]
  }