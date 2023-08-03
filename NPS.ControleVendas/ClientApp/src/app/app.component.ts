import { Component, OnInit } from '@angular/core'; 
import { Constants } from './config/constants';
import { AuthApiService } from './services/auth-api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = Constants.AppName;
  globalConstantes = new Constants();
  /**
   *
   */
  constructor( 
    private authApiService: AuthApiService
  ) {
    console.log(this.globalConstantes.API_AUTH_ENDPOINT);
  }

  ngOnInit(): void {
    console.log(this.title); 
    console.log("pasei aqui"); 
    this.authApiService.getToken("sebastiao.pereira","1234123");
    console.log("e depois passei aqui"); 
  }
 
}
