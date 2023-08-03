import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthApiService } from 'src/app/services/auth-api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

  onLogin() {
    this.authApiService.getToken("sebastiao.pereira","sebastiao");
  }

  constructor(
     private authApiService: AuthApiService
  ) {
    debugger;
  }

  ngOnInit() {}

  ngOnDestroy() {}

}
