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
import { of } from 'rxjs';
import { ProductionItem } from 'src/app/shared/models/productionItem';
import { flatMap } from 'rxjs/operators';
import { ProductionSummary } from 'src/app/shared/models/productionSummary';

@Component({
  selector: 'app-data-sheet-details',
  templateUrl: './data-sheet-details.component.html',
  styleUrls: ['./data-sheet-details.component.scss']
})
export class DataSheetDetailsComponent implements OnInit {

  public title = 'Ficha Técnica';
  public product: Product = {} as Product;
  public dataSheet: DataSheet = {} as DataSheet;
  public currentItem: DataSheetItem = {} as DataSheetItem;
  public measures: MeasureUnit[] = [];
  public summary: ProductionSummary = { profitPercentage: 0, productionCost: 0, productionItems: [], profit: 0, salePrice: 0 };
  public production = { quantity: 1, measureUnit: 1 };
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
    let id = this.route.snapshot.params.id;
    let oProduct = this.productService.GetProduct(id);
    let oDataSheet = this.productService.getDataSheet(id);
    let oMeasures = this.domainService.GetMeasureUnits();
    forkJoin(oProduct, oDataSheet, oMeasures).pipe(flatMap(r => {
      this.product = r[0];
      this.title = `${this.product.name} / Ficha Técnica`;
      this.dataSheet = r[1] ?? { dataSheetId: null, dataSheetItems: [], procedureDescription: null, productId: this.product.productId, weightVariationPercentage: 0, increaseWeight: true };
      this.measures = r[2] ?? [];
      this.spinner.hide();
      let result: ProductionItem[] = [];
      this.production.measureUnit = this.product.measureUnitId;
      this.dataSheet.dataSheetItems.forEach(i => {
        let item = { ...i } as ProductionItem;
        item.quantity = 0;
        item.cost = 0;
        result.push(item);
      })
      return this.productService.calculateProduction(this.product.productId, this.production.measureUnit, this.production.quantity)

    })).subscribe(r => {
      this.summary = r;
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
