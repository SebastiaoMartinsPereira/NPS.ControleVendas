import { Component, Input, OnInit } from '@angular/core';
import { Alert } from 'src/app/models/alert';

@Component({
  selector: 'app-alert', 
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent implements OnInit {
  @Input() 
  alert: Alert; 
  timeOutWindow: any;
   
  
  constructor() {

  }

  ngOnInit(): void {}
  
  show() {
    console.log("Show:", JSON.stringify(this.alert));
    if (this.alert.timeToHide >= 0 && this.alert.show) {

      if(this.timeOutWindow>0){
        clearTimeout(this.timeOutWindow)
      }

      this.timeOutWindow = setTimeout(() => {
        this.alert = {
          type: "info",
          show: false,
          header: "",
          message: "",
          timeToHide: 2
        };
      }, this.alert.timeToHide * 1000);
    }
    return this.alert.show;
  }
}
