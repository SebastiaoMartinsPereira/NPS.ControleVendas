import { Injectable } from "@angular/core"; 
import { Constants } from "../config/constants";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable } from "rxjs"; 
import { BaseHttpService } from "./basehttp.service";
import { HttpClient } from "@angular/common/http";
import { LogedUser } from "../models/logedUser";

@Injectable({ providedIn: 'root' })
export class AuthApiService extends BaseHttpService<LogedUser> {

    getTokenEndPoint;
    userSubject: any;
    user: any;
    
    /**
     *
     */
    constructor(
        http: HttpClient,
        router: Router,
    ) {
        super(http,router);
        
        let constants = new Constants();

        this.userSubject = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
        this.user = this.userSubject.asObservable();
        this.getTokenEndPoint =`Auth/token`;
    }

    public get userValue() {
        return this.userSubject.value;
    }

    public getToken(email:string , pass:string ): Observable<LogedUser>{
          const sendBody: string = JSON.stringify({
            "Login": email, 
            "Password": pass
          });
          return this.post(this.getTokenEndPoint, sendBody);
    }

    public logout() {
        // remove user from local storage and set current user to null
        localStorage.removeItem('user');
        this.userSubject.next(null);
        this.router.navigate(['/account/login']);
    }
}