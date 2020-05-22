import { Component, OnInit } from '@angular/core';
import { PaginationService } from '../services/pagination/pagination.service';
import { PaginationInfo } from '../models/paginationInfo';

@Component({
  selector: 'app-server-side-pager',
  templateUrl: './server-side-pager.component.html',
  styleUrls: ['./server-side-pager.component.scss']
})
export class ServerSidePagerComponent implements OnInit {
  public paginationInfo: PaginationInfo;
  public paginationButtons: number[] = [];
  constructor(private paginationService: PaginationService) {
    this.paginationService.onChangePage.subscribe(r => this.paginationInfo = r);
    this.paginationService.onPaginationUpdate.subscribe(r => {
      this.paginationButtons = r.buttons;
      this.paginationInfo = r.pagingInfo;
    });
  }
  ngOnInit(): void {
  }
  first() {
    this.paginationService.first();
  }
  previous() {
    this.paginationService.first();
  }
  changePage(page: number) {
    this.paginationService.changePage(page);
  }
  next() {
    this.paginationService.next();
  }
  last() {
    this.paginationService.last();
  }
}
