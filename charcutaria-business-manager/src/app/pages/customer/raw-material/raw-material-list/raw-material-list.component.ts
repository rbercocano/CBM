import { Component, OnInit, ViewChild } from '@angular/core';
import { PaginationInfo } from 'src/app/shared/models/paginationInfo';
import { NgxSpinnerService } from 'ngx-spinner';
import { PaginationService } from 'src/app/shared/services/pagination/pagination.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { PagedResult } from 'src/app/shared/models/pagedResult';
import { RawMaterial } from 'src/app/shared/models/rawMaterial';
import { RawMaterialService } from 'src/app/shared/services/rawMaterial/raw-material.service';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PricingService } from 'src/app/shared/services/pricing/pricing.service';
import { PricingRequest } from 'src/app/shared/models/pricingRequest';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin } from 'rxjs';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { flatMap } from 'rxjs/operators';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-raw-material-list',
  templateUrl: './raw-material-list.component.html',
  styleUrls: ['./raw-material-list.component.scss']
})
export class RawMaterialListComponent implements OnInit {
  @ViewChild("f") form: NgForm;
  public paginationInfo: PaginationInfo = {} as PaginationInfo;
  private modal: NgbModalRef;
  public filter = "";
  public active: boolean = null;
  public rawMaterials: RawMaterial[] = [];
  public measures: MeasureUnit[] = [];
  public sortDirection = 1;
  public currentRawMaterial: RawMaterial = {} as RawMaterial;
  public quote: PricingRequest = { resultPrecision: 4, quantity: 0, productMeasureUnit: 1, productPrice: 0, quantityMeasureUnit: 1 };
  public pricePaid: number;
  constructor(private rawMaterialService: RawMaterialService,
    private spinner: NgxSpinnerService,
    private pricingService: PricingService,
    private domainService: DomainService,
    private paginationService: PaginationService,
    private modalService: NgbModal,
    private notificationService: NotificationService) {
    this.paginationInfo.currentPage = 1;
    this.paginationInfo.recordsPerpage = 10;
    this.paginationService.onChangePage.subscribe(r => {
      if (r != null)
        this.search();
    });
  }
  ngOnInit(): void {
    let oRawMaterial = this.rawMaterialService.GetPaged(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter, this.sortDirection);
    let oMeasureUnit = this.domainService.GetMeasureUnits();

    forkJoin(oRawMaterial, oMeasureUnit).subscribe(r => {
      this.measures = r[1] ?? [];
      this.currentRawMaterial.measureUnitId = this.measures[0].measureUnitId;
      let info: PagedResult<RawMaterial> = r[0] ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
      this.rawMaterials = info.data;
      this.paginationInfo = info;
      this.paginationService.updatePaging(info);
      this.spinner.hide();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  newSearch() {
    this.paginationInfo.currentPage = 1;
    this.sortDirection = 1;
    this.search();
  }
  search() {
    this.spinner.show();
    this.rawMaterials = [];
    this.rawMaterialService
      .GetPaged(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter, this.sortDirection)
      .subscribe(r => {
        let info: PagedResult<RawMaterial> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.rawMaterials = info.data;
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.spinner.hide();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  changePageSize() {
    this.paginationService.changePageSize(this.paginationInfo.recordsPerpage);
  }

  open(content) {
    this.pricePaid = 0;
    this.quote = { resultPrecision: 4, quantity: 0, productMeasureUnit: 1, productPrice: 0, quantityMeasureUnit: 1 };
    this.modal = this.modalService.open(content);
  }
  calculatePrice() {
    this.pricingService.calculateRawMaterialPrice(this.quote).subscribe(r => {
      this.pricePaid = r;
      this.currentRawMaterial.price = r;
      this.currentRawMaterial.measureUnitId = this.quote.quantityMeasureUnit;
    });
  }
  sort() {
    this.sortDirection = this.sortDirection == 1 ? 2 : 1;
    this.search();
  }
  save() {
    if (!this.form.valid) return;
    this.spinner.show();
    if (this.currentRawMaterial.rawMaterialId == null)
      this.add();
    else
      this.update();
  }
  add() {
    this.rawMaterialService.Add(this.currentRawMaterial)
      .pipe(flatMap(r => {
        this.paginationInfo.currentPage = 1;
        this.sortDirection = 1;
        this.filter = null;
        this.form.resetForm();
        this.currentRawMaterial = {} as RawMaterial;
        this.currentRawMaterial.measureUnitId = this.measures[0].measureUnitId;
        this.notificationService.showSuccess('Sucesso', 'Matéria Prima adicionada com sucesso');
        return this.rawMaterialService.GetPaged(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter, this.sortDirection);
      })).subscribe(r => {
        let info: PagedResult<RawMaterial> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.rawMaterials = info.data;
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.spinner.hide();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  update() {
    this.rawMaterialService.Update(this.currentRawMaterial)
      .pipe(flatMap(r => {
        this.paginationInfo.currentPage = 1;
        this.filter = null;
        this.sortDirection = 1;
        this.form.resetForm();
        this.currentRawMaterial = {} as RawMaterial;
        this.currentRawMaterial.measureUnitId = this.measures[0].measureUnitId;
        this.notificationService.showSuccess('Sucesso', 'Matéria Prima atualizada com sucesso');
        return this.rawMaterialService.GetPaged(this.paginationInfo.currentPage, this.paginationInfo.recordsPerpage, this.filter, this.sortDirection);
      })).subscribe(r => {
        let info: PagedResult<RawMaterial> = r ?? { data: [], currentPage: 1, recordCount: 0, recordsPerpage: this.paginationInfo.recordsPerpage, totalPages: 0 };
        this.rawMaterials = info.data;
        this.paginationInfo = info;
        this.paginationService.updatePaging(info);
        this.spinner.hide();
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  edit(obj: RawMaterial) {
    this.currentRawMaterial = { ...obj };
  }
  public get quotedMeasureUnit(): string {
    return this.measures.filter(m => m.measureUnitId == this.quote.quantityMeasureUnit)[0].description;
  }
}
