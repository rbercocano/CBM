import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { IConfirmModal } from './confirmModal';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.scss']
})
export class ConfirmModalComponent implements OnInit {
  @Input("data") data: any;
  @Output("confirm") onConfirm = new EventEmitter<any>();
  @ViewChild("content", { static: true }) content: any;
  private modal: NgbModalRef;
  constructor(private modalService: NgbModal) { }
  ngOnInit(): void {
  }

  confirm() {
    this.onConfirm.emit(this.data);
  }
  public open() {
    this.modal = this.modalService.open(this.content, { size: 'lg' });
  }
  close() {
    if (this.modal)
      this.modal.close()
  }
}
