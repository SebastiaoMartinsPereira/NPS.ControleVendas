import { Component, OnInit } from '@angular/core';
import { AlertComponent } from '../alert/alert.component';

@Component({
  selector: 'app-alert-with-buton',
  templateUrl: './alert-with-buton.component.html',
  styleUrl: './alert-with-buton.component.scss'
})
export class AlertWithButonComponent extends AlertComponent implements OnInit {
  constructor(){  
    super();
  }

  ngOnInit(): void {
    debugger;
    this.childComponent = "AlertWithButonComponent";
    this.alert.timeToHide = -1;
    super.ngOnInit();
  }

  show(): boolean{
    debugger;
    this.alert.timeToHide = -1;
    return super.show();
  }

  hide(){
    this.alert.show = false;
    this.alert.message = "";
    this.alert.header = "";
  }
}