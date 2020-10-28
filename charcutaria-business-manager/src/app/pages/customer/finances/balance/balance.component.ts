import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Balance } from 'src/app/shared/models/balance';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { TransactionService } from 'src/app/shared/services/transaction/transaction.service';
import * as moment from 'moment';
import { TransactionType } from 'src/app/shared/models/transactionType';
import { NewTransaction } from 'src/app/shared/models/newTransaction';
import { flatMap } from 'rxjs/operators';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin } from 'rxjs';
import { NgbDateStruct, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.scss']
})
export class BalanceComponent implements OnInit {
  @ViewChild('transModal', { static: true }) transModal: any;
  @ViewChild('searchModal', { static: true }) searchModal: any;
  private modal: NgbModalRef;
  public filteredTypes: TransactionType[] = [];
  private types: TransactionType[] = [];
  public inOut = [{ id: 'I', desc: 'Receita' }, { id: 'O', desc: 'Despesa' }];
  public selectedInOut = 'I';
  public start: string;
  public end: string;
  public balance: Balance[] = [];
  public totalBalance: number = 0;
  public transaction: NewTransaction = {} as NewTransaction;
  public maxDate: NgbDateStruct;
  public time = { hour: 0, minute: 0 };
  constructor(private transactionService: TransactionService,
    private modalService: NgbModal,
    private notificationService: NotificationService,
    private spinner: NgxSpinnerService,
    private domainService: DomainService) {
    let today = moment(new Date());
    this.maxDate = {
      day: parseInt(today.format('DD')),
      month: parseInt(today.format('MM')),
      year: parseInt(today.format('YYYY'))
    };
    this.time = {
      hour: parseInt(today.format('HH')),
      minute: parseInt(today.format('mm'))
    };
    this.end = today.format("YYYY-MM-DDTHH:mm:ss");
    let dStart = today.subtract(30, 'd');
    this.start = dStart.format("YYYY-MM-DDTHH:mm:ss");
  }
  ngOnInit(): void {
    var oTypes = this.domainService.GetTransactionTypes();
    this.spinner.show();
    var oBal = this.transactionService.GetBalance(this.start, this.end);
    var oTotalBal = this.transactionService.GetTotalBalance();
    forkJoin([oTypes, oBal, oTotalBal]).subscribe(r => {
      this.types = r[0] ?? [];
      this.balance = r[1] ?? [];
      this.totalBalance = r[2];
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }

  public search(): void {
    this.modal.close();
    this.spinner.show();
    this.transactionService.GetBalance(this.start, this.end).subscribe(r => {
      this.balance = r ?? [];
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  public add(f: NgForm): void {
    if (!f.form.valid) return;
    this.transaction.date = this.transactionDateTime;
    this.modal.close();
    this.spinner.show();
    this.transactionService.Add(this.transaction)
      .pipe(flatMap(r => {
        var oBal = this.transactionService.GetBalance(this.start, this.end);
        var oTot = this.transactionService.GetTotalBalance();
        return forkJoin([oBal, oTot]);
      }))
      .subscribe(r => {
        this.balance = r[0] ?? [];
        this.totalBalance = r[1];
        this.transaction = {} as NewTransaction;
        this.spinner.hide();
      }, e => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  filterTypes() {
    this.filteredTypes = [];
    this.types.forEach((v, i) => {
      if (v.type == this.selectedInOut) {
        this.filteredTypes.push(v);
      }
    });
    this.transaction.transactionTypeId = this.filteredTypes[0].transactionTypeId;
  }
  openTransactionModal() {
    this.filterTypes();
    this.transaction.date = moment(new Date()).format("YYYY-MM-DDTHH:mm:ss");
    this.modal = this.modalService.open(this.transModal, { size: 'lg' });
  }
  openSearchModal() {
    this.modal = this.modalService.open(this.searchModal, { size: 'lg' });
  }
  private get transactionDateTime(): string {
    let date = moment(this.transaction.date);
    let d = {
      day: parseInt(date.format("DD")),
      month: parseInt(date.format("MM")),
      year: parseInt(date.format("YYYY")),
      hour: this.time.hour,
      minute: this.time.minute
    };
    var result = moment(`${d.year}-${d.month}-${d.day} ${d.hour}:${d.minute}:00`);
    return result.format("YYYY-MM-DDTHH:mm:ss");
  }
  public get totalDays(): number {
    var dstart = moment(this.start);
    var dend = moment(this.end);
    return dend.diff(dstart, 'days')
  }
  public get canSearch(): boolean {
    return this.totalDays <= 180;
  }
}
