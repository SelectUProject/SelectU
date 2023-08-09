import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
})
export class MainLayoutComponent implements OnInit {
  ticketPage: boolean = false;
  campaignPage: boolean = false;
  loaded: boolean = false;

  constructor(public router: Router) {
    this.loaded = true;
  }

  ngOnInit(): void {}
}
