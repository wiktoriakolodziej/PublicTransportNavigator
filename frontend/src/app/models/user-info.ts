import { DiscountDTO } from "./discount";
import { FavouriteBusStopDTO } from "./favourite-bus-stop";

export interface UserInfo {
    id: number,
    name: string,
    surname: string,
    favouriteBusStops?: FavouriteBusStopDTO[]; 
    discounts?: DiscountDTO[];
  }