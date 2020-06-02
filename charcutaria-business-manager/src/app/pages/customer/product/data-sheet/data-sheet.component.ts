import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';
import { ProductService } from 'src/app/shared/services/product/product.service';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, ActivatedRoute } from '@angular/router';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { forkJoin, of } from 'rxjs';
import { Product } from 'src/app/shared/models/product';
import { DataSheetItem } from 'src/app/shared/models/dataSheetItem';
import { DataSheet } from 'src/app/shared/models/dataSheet';
import { RawMaterialService } from 'src/app/shared/services/rawMaterial/raw-material.service';
import { RawMaterial } from 'src/app/shared/models/rawMaterial';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { flatMap } from 'rxjs/operators';
import { NewDataSheetItem } from 'src/app/shared/models/newDataSheetItem';
import { UpdateDataSheetItem } from 'src/app/shared/models/updateDataSheetItem';

@Component({
  selector: 'app-data-sheet',
  templateUrl: './data-sheet.component.html',
  styleUrls: ['./data-sheet.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DataSheetComponent implements OnInit {
  public title = 'Ficha Técnica';
  public options: Object = {
    placeholderText: 'Digite as informações do procedimento aqui!',
    charCounterCount: false,
    quickInsertEnabled: false,
    height: 400,
    toolbarButtons: {
      'moreText': {
        'buttons': ['bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize', 'textColor', 'backgroundColor', 'inlineClass', 'inlineStyle', 'clearFormatting']
      },
      'moreParagraph': {
        'buttons': ['alignLeft', 'alignCenter', 'formatOLSimple', 'alignRight', 'alignJustify', 'formatOL', 'formatUL', 'paragraphFormat', 'paragraphStyle', 'lineHeight', 'outdent', 'indent', 'quote']
      },
      'moreRich': {
        'buttons': ['insertLink', 'insertTable', 'specialCharacters', 'insertHR']
      },
      'moreMisc': {
        'buttons': ['undo', 'redo', 'fullscreen', 'print', 'selectAll'],
        'align': 'right',
        'buttonsVisible': 2
      }
    }
  }
  private modal: NgbModalRef;
  public product: Product = {} as Product;
  public dataSheet: DataSheet = {} as DataSheet;
  public rawMaterials: RawMaterial[] = [];
  public currentItem: DataSheetItem = {} as DataSheetItem;
  constructor(private productService: ProductService,
    private modalService: NgbModal,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private notificationService: NotificationService,
    private rawMaterialService: RawMaterialService) {
    this.currentItem.percentage = 0;
    this.dataSheet.dataSheetItems = [];
  }

  ngOnInit(): void {
    this.spinner.show();
    let id = this.route.snapshot.params.id;
    let oProduct = this.productService.GetProduct(id);
    let oDataSheet = this.productService.getDataSheet(id);
    let oMaterial = this.rawMaterialService.GetAll(1);
    forkJoin(oProduct, oDataSheet, oMaterial).subscribe(r => {
      this.product = r[0];
      this.title = `${this.product.name} / Ficha Técnica`;
      this.dataSheet = r[1] ?? { dataSheetId: null, dataSheetItems: [], procedureDescription: null, productId: this.product.productId };
      this.rawMaterials = r[2] ?? [];
      this.spinner.hide();
    },
      (e) => {
        this.spinner.hide();
        this.notificationService.notifyHttpError(e);
      });
  }

  back() {
    this.router.navigate(['/product']);
  }
  save(){
    this.spinner.show();
    this.productService.saveDataSheet(this.dataSheet).pipe(flatMap(r => {
      return this.productService.getDataSheet(this.product.productId);
    })).subscribe(r => {
      this.dataSheet = r;
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso','Procedimento atualizado com sucesso.');
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });

  }
  addItem() {
    this.modal.dismiss();
    this.spinner.show();
    let oCreate = this.dataSheet.dataSheetId == null ?
      this.productService.saveDataSheet(this.dataSheet) : of(this.dataSheet.dataSheetId);
    oCreate.pipe(flatMap(r => {
      this.dataSheet.dataSheetId = r;
      let newItem: NewDataSheetItem = {
        additionalInfo: this.currentItem.additionalInfo,
        percentage: this.currentItem.percentage,
        rawMaterialId: this.currentItem.rawMaterialId,
        isBaseItem: this.currentItem.isBaseItem,
        productId: this.product.productId
      };
      return this.productService.addDataSheetItem(newItem);
    })).pipe(flatMap(r => {
      return this.productService.getDataSheet(this.product.productId);
    })).subscribe(r => {
      this.spinner.hide();
      this.dataSheet = r;
      this.notificationService.showSuccess('Sucesso','Ingrediente adicionado com sucesso.');
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  saveItem() {
    this.modal.dismiss();
    this.spinner.show();
    let item: UpdateDataSheetItem = {
      additionalInfo: this.currentItem.additionalInfo,
      percentage: this.currentItem.percentage,
      rawMaterialId: this.currentItem.rawMaterialId,
      isBaseItem: this.currentItem.isBaseItem,
      productId: this.product.productId,
      dataSheetItemId: this.currentItem.dataSheetItemId
    };
    this.productService.updateDataSheetItem(item).pipe(flatMap(r => {
      return this.productService.getDataSheet(this.product.productId);
    })).subscribe(r => {
      this.dataSheet = r;
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso','Ingrediente atualizado com sucesso.');
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  open(content, item: DataSheetItem) {
    if (item == null)
      this.resetModal();
    else {
      this.currentItem = { ...item };
    }
    this.modal = this.modalService.open(content);
  }
  resetModal() {
    this.currentItem = {} as DataSheetItem;
    this.currentItem.percentage = 0;
    this.currentItem.isBaseItem = true;
    if (this.rawMaterials.length > 0)
      this.currentItem.rawMaterialId = this.rawMaterials[0].rawMaterialId;
  }
  public get baseItems(): DataSheetItem[] {
    return (this.dataSheet.dataSheetItems ?? []).filter(i => i.isBaseItem);
  }
  public get otherItems(): DataSheetItem[] {
    return (this.dataSheet.dataSheetItems ?? []).filter(i => !i.isBaseItem);
  }
  datasheetDetails() {
    this.router.navigate(['/product', this.product.productId, 'datasheet','details']);
  }
  remove(item) {
    this.spinner.show();
    this.productService.deleteDataSheetItem(item.dataSheetItemId).pipe(flatMap(r => {
      return this.productService.getDataSheet(this.product.productId);
    })).subscribe(r => {
      this.dataSheet = r;
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso','Ingrediente removido com sucesso.');
    }, (e) => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
}
