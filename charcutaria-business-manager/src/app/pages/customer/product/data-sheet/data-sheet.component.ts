import { Component, OnInit } from '@angular/core';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { forkJoin } from 'rxjs';
import { Product } from 'src/app/shared/models/product';
import { DataSheetItem } from 'src/app/shared/models/dataSheetItem';
import { DataSheet } from 'src/app/shared/models/dataSheet';

@Component({
  selector: 'app-data-sheet',
  templateUrl: './data-sheet.component.html',
  styleUrls: ['./data-sheet.component.scss']
})
export class DataSheetComponent implements OnInit {
  public title = 'Ficha Técnica';
  public product: Product = {} as Product;
  public dataSheet: DataSheet = {} as DataSheet;
  constructor(private productService: ProductService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    let id = this.route.snapshot.params.id;
    let oProduct = this.productService.GetProduct(id);
    let oDataSheet = this.productService.getDataSheet(id);
    forkJoin(oProduct, oDataSheet).subscribe(r => {
      this.product = r[0];
      this.dataSheet = r[1] ?? {} as DataSheet;
    },
      (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }

  back() {
    this.router.navigate(['/product']);
  }
}
