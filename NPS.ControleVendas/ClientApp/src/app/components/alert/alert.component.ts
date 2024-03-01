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
  childComponent: string;

  constructor( ) {
    this.childComponent;
  }

  ngOnInit(): void { 
    var a =1;
  }

  show(): boolean{
    console.log("Show:", JSON.stringify(this.alert));
    console.log("this.childComponent:", this.childComponent);
    if (this.alert.timeToHide >= 0 && this.alert.show) {

      if (this.timeOutWindow > 0) {
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
