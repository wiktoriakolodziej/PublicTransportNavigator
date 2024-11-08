import { BusTimetableDTO } from "./bus-timetable";

export interface BusStopDetailsDTO {
    id: number;
    name: string;
    onRequest: boolean;
    buses: BusTimetableDTO[];
  }