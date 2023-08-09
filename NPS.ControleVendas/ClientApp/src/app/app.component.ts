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
    
  }

  ngOnInit(): void {
    console.log(this.title);
  }
 
}
