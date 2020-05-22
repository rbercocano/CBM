import { Component, OnInit, ViewChild } from '@angular/core';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { NgxSpinnerService } from "ngx-spinner";
import { Router, ActivatedRoute } from '@angular/router';
import { Product } from 'src/app/shared/models/product';
import { of, forkJoin } from 'rxjs';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { NgForm } from '@angular/forms';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  public title: string;
  public product: Product = {
    activeForSale: true,
    measureUnit: null,
    measureUnitId: null,
    name: null,
    price: null,
    productId: null,
    corpClientId: null
  };
  public measures: MeasureUnit[] = [];
  constructor(private productService: ProductService,
    private domainService: DomainService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.spinner.show();
    this.product.productId = this.route.snapshot.params.id;
    this.title = this.product.productId ? 'Editar Produtos' : 'Cadastrar Produto';
    let obsProd = this.product.productId ? this.productService.GetProduct(this.product.productId) : of(this.product);
    forkJoin(obsProd, this.domainService.GetMeasureUnits()).subscribe(r => {
      this.product = r[0];
      this.measures = r[1];
      this.spinner.hide();
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }

  back() {
    this.router.navigate(['/product']);
  }
  save() {
    if (!this.form.valid) return;
    this.spinner.show();
    if (!this.product.productId)
      this.productService
        .Add(this.product)
        .subscribe(r => {
          this.product.productId = r;
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Produto salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
    else
      this.productService
        .Update(this.product)
        .subscribe(r => {
          this.product = r;
          this.spinner.hide();
          this.notificationService.showSuccess('Sucesso', 'Produto salvo com sucesso.');
        }, (e) => {
          this.spinner.hide();
          this.notificationService.notifyHttpError(e);
        });
  }
}
