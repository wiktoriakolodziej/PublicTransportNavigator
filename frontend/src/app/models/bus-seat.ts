import { CoordinateDTO } from "./coordinateDTO";

export interface BusSeatDTO {
    id: number;
    busId: number;
    seatType: string;
    coordinate: CoordinateDTO;
    available: boolean;
    imagePath: string;
  }