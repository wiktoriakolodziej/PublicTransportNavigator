import { UserInfo } from "./user-info";

export interface LoginResponseDTO {
    token: string,
    user: UserInfo,
    expirationTime: number,
}