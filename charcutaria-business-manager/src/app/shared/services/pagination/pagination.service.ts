import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PaginationInfo } from '../../models/paginationInfo';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {  
  private paginationInfo: PaginationInfo;

  private pageChangeSource = new BehaviorSubject<PaginationInfo>(null);
  public onChangePage = this.pageChangeSource.asObservable();

  private paginationUpdateSource = new BehaviorSubject({ buttons: [], pagingInfo: this.paginationInfo });
  public onPaginationUpdate = this.paginationUpdateSource.asObservable();

  changePage(newPage: number) {
    if (newPage == this.paginationInfo.currentPage) return;
    this.paginationInfo.currentPage = newPage;
    this.pageChangeSource.next(this.paginationInfo);
  }
  changePageSize(newSize: number) {
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = newSize;
    this.pageChangeSource.next(this.paginationInfo);
  }
  first() {
    this.paginationInfo.currentPage = 1;
    this.pageChangeSource.next(this.paginationInfo);
  }
  last() {
    this.paginationInfo.currentPage = this.paginationInfo.totalPages;
    this.pageChangeSource.next(this.paginationInfo);
  }
  next() {
    if (this.paginationInfo.currentPage == this.paginationInfo.totalPages)
      return;
    this.paginationInfo.currentPage++;
    this.pageChangeSource.next(this.paginationInfo);
  }
  previous() {
    if (this.paginationInfo.currentPage == 1)
      return;
    this.paginationInfo.currentPage--;
    this.pageChangeSource.next(this.paginationInfo);
  }
  updatePaging(paginationInfo: PaginationInfo) {
    this.paginationInfo = paginationInfo;
    var buttons = this.getButtons();
    this.paginationUpdateSource.next({ buttons: buttons, pagingInfo: this.paginationInfo });
  }
  private getButtons(): number[] {
    let paginationButtons = [];
    let max = 3;
    let start = 1;
    let end = max;
    if (this.paginationInfo.currentPage - 1 <= 0) {
      start = 1;
      end = start + 2;
    } else if (this.paginationInfo.currentPage + 1 >= this.paginationInfo.totalPages) {
      start = this.paginationInfo.totalPages - 2;
      end = this.paginationInfo.totalPages;
    } else {
      start = this.paginationInfo.currentPage - 1;
      end = this.paginationInfo.currentPage + 1;
    }
    end = (end - start + 1) > this.paginationInfo.totalPages ? this.paginationInfo.totalPages : end;
    start = start <= 0 ? 1 : start;
    for (let i = start; i <= end; i++)
      paginationButtons.push(i);
    return paginationButtons;
  }
}
