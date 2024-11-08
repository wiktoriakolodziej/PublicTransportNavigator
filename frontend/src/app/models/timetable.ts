export interface TimetableDTO {
    id: number;
    busId: number;
    busName?: string;
    busStopId: number;
    busStopName?: string;
    time: string;
  }