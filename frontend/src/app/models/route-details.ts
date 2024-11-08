import { TimetableDTO } from "./timetable";

export interface RouteDetailsDTO {
    id: number;
    timetableIn: TimetableDTO;
    timetableOut: TimetableDTO;
    date: string;
  }