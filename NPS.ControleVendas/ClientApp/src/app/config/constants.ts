import { Injectable } from "@angular/core";

@Injectable()
export class Constants {
    public static AppName: string = 'NPS.ControleVendas';
    public readonly API_AUTH_ENDPOINT: string = 'https://localhost:7167';
}