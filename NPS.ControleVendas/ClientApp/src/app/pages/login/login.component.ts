import { Component, OnInit, OnDestroy } from '@angular/core';
import { finalize } from 'rxjs';
import { AuthApiService } from 'src/app/services/auth-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

  onLogin(password: any,login: any) {
    console.log(password,login);
    if(password && login){
      this.authApiService.getToken(login,password).pipe(
        finalize(() => {
          console.log("funcionou");
        })
      ).subscribe((ret: any) => {
        console.log(ret)
      }, error => {
        console.log(error)
      });
 
    }
  }

  constructor(
     private authApiService: AuthApiService
  ) {
    debugger;
  }

  ngOnInit() {}

  ngOnDestroy() {}

}
