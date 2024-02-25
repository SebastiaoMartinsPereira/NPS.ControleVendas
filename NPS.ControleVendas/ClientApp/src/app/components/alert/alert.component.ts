import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert', 
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss'
})
export class AlertComponent implements OnInit {
  @Input() 
  show: boolean | null = false;
  constructor() { }

  ngOnInit(): void {
  }
  
}
