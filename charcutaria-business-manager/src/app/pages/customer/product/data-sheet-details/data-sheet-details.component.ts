import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';
import { Product } from 'src/app/shared/models/product';
import { DataSheet } from 'src/app/shared/models/dataSheet';
import { DataSheetItem } from 'src/app/shared/models/dataSheetItem';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { ProductionItem } from 'src/app/shared/models/productionItem';
import { flatMap, tap } from 'rxjs/operators';
import { ProductionSummary } from 'src/app/shared/models/productionSummary';
import { of } from 'rxjs';

@Component({
  selector: 'app-data-sheet-details',
  templateUrl: './data-sheet-details.component.html',
  styleUrls: ['./data-sheet-details.component.scss']
})
export class DataSheetDetailsComponent implements OnInit {

  public product: Product = {} as Product;
  public products: Product[] = [];
  public dataSheet: DataSheet = {} as DataSheet;
  public currentItem: DataSheetItem = {} as DataSheetItem;
  public measures: MeasureUnit[] = [];
  public summary: ProductionSummary = { profitPercentage: 0, productionCost: 0, productionItems: [], profit: 0, salePrice: 0 };
  public production = { quantity: 1, measureUnit: 1, productId: 0 };
  constructor(private productService: ProductService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private domainService: DomainService) {
    this.currentItem.percentage = 0;
    this.dataSheet.dataSheetItems = [];
  }
  ngOnInit(): void {
    this.spinner.show();
    let state = history.state.data;
    if (state != null) {
    }
    this.productService.GetAll().pipe(
      flatMap(r => {
        this.products = r ?? [];
        let oDataSheet = of(this.dataSheet);
        let oMeasures = of(this.measures);

        if (this.products.length == 0)
          return forkJoin([oDataSheet, oMeasures]);

        if (state != null) {
          this.production.quantity = state.quantity;
          this.production.measureUnit = state.measureUnitId;
          this.production.productId = state.productId;
        } else
          this.production.productId = this.products[0].productId;

        var prod = this.products.filter(p => p.productId == this.production.productId);
        this.product = prod[0];
        oDataSheet = this.productService.getDataSheet(this.product.productId);
        oMeasures = this.domainService.GetMeasureUnits();
        return forkJoin([oDataSheet, oMeasures]);
      }),
      flatMap(r => {
        if (state == null) {
          this.production.measureUnit = this.product.measureUnitId;
          this.production.quantity = 1;
        }
        this.dataSheet = r[0] ?? { dataSheetId: null, dataSheetItems: [], procedureDescription: null, productId: this.product.productId, weightVariationPercentage: 0, increaseWeight: true };
        this.measures = r[1] ?? [];

        if (this.products.length == 0)
          return of(this.summary);

        return this.productService.calculateProduction(this.product.productId, this.production.measureUnit, this.production.quantity);
      })).subscribe(r => {
        this.spinner.hide();
        this.summary = r;
      }, (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }
  getDataSheet() {
    this.spinner.show();
    let oSummary = this.productService.calculateProduction(this.product.productId, this.production.measureUnit, this.production.quantity);
    let oDataSheet = this.productService.getDataSheet(this.product.productId);
    forkJoin([oDataSheet, oSummary]).subscribe(r => {
      this.dataSheet = r[0] ?? { dataSheetId: null, dataSheetItems: [], procedureDescription: null, productId: this.product.productId, increaseWeight: true, weightVariationPercentage: 0 };
      this.summary = r[1];
      this.spinner.hide();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  back() {
    this.router.navigate(['/product']);
  }
  datasheet() {
    this.router.navigate(['/product', this.product.productId, 'datasheet']);
  }
  public get baseItems(): DataSheetItem[] {
    return this.summary.productionItems.filter(i => i.isBaseItem);
  }
  public get otherItems(): DataSheetItem[] {
    return this.summary.productionItems.filter(i => !i.isBaseItem);
  }
  public get title(): string {
    return this.product != null ? `${this.product.name} / Ficha Técnica` : 'Ficha Técnica';
  }
  calculate() {
    this.spinner.show();
    this.productService.calculateProduction(this.product.productId, this.production.measureUnit, this.production.quantity).subscribe(r => {
      this.summary = r;
      this.spinner.hide();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
}
