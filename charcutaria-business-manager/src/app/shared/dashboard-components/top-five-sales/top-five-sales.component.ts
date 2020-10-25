import { Component, Input, OnInit } from '@angular/core';
import { Production } from '../../models/production';

@Component({
  selector: 'top-five-sales',
  templateUrl: './top-five-sales.component.html',
  styleUrls: ['./top-five-sales.component.scss']
})
export class TopFiveSalesComponent implements OnInit {
  @Input("data") production: Production[] = [];
  constructor() { }

  ngOnInit(): void {
  }

}
