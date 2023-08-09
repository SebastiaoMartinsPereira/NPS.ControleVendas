import { Claim } from "./claim";

export interface LogedUser {
    displayName: string;
    userName: string;
    email: string;
    token: string;
    claims: Claim[];
}
