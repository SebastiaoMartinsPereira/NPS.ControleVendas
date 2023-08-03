import { Injectable } from "@angular/core";
import { ApiHttpService } from "../core/api-http.service";
import { Constants } from "../config/constants";

@Injectable()
export class AuthApiService {

    getTokenEndPoint;
    
    /**
     *
     */
    constructor(
        private httpService: ApiHttpService 
    ) {
        let constants = new Constants();
        this.getTokenEndPoint =`${constants.API_AUTH_ENDPOINT}/api/Auth/token`;
    }

    public getToken(email:string , pass:string ){
        var t = this.httpService.post(this.getTokenEndPoint,{
            "email": email, 
            "password": pass
          }).subscribe(
            value => { console.log(value) },
            error=> {console.log(error)}
          );
    }
}
