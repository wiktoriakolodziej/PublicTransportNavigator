export interface ReservdSeatCreateDTO {
    busSeatId: number;
    timetableInId: number;
    timetableOffId: number;
    userTravelId?: number;
    reservationDate: string;
  }
  