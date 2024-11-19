export interface ReservdSeatCreateDTO {
    busSeatId: number;
    busIdIn: number;
    timeIn: string;
    timeOff: string;
    userTravelId?: number;
    reservationDate: string;
  }
  