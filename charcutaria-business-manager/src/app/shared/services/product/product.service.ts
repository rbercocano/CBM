import { Injectable } from '@angular/core';
import { Product } from '../../models/product';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PagedResult } from '../../models/pagedResult';
import { DataSheet } from '../../models/dataSheet';
import { DataSheetItem } from '../../models/dataSheetItem';
import { SaveDataSheet } from '../../models/saveDataSheet';
import { NewDataSheetItem } from '../../models/newDataSheetItem';
import { UpdateDataSheetItem } from '../../models/updateDataSheetItem';
import { ProductionItem } from '../../models/productionItem';
import { ProductionSummary } from '../../models/productionSummary';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClient: HttpClient) { }
  public GetProduct(ProductId: number): Observable<Product> {
    return this.httpClient.get<Product>(`${environment.apiUrl}/Product/${ProductId}`);
  }

  public GetPaged(page: number, pageSize: number, filter: string, active: boolean | null): Observable<PagedResult<Product>> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    if (active != null)
      params = params.append('active', String(active));
    return this.httpClient.get<PagedResult<Product>>(`${environment.apiUrl}/Product/${page}/${pageSize}`, { params: params });
  }
  public Add(product: Product): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Product`, product);
  }
  public Update(product: Product): Observable<Product> {
    return this.httpClient.put<Product>(`${environment.apiUrl}/Product`, product);
  }
  public GetAll(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(`${environment.apiUrl}/Product`);
  }
  public getDataSheet(productId: number): Observable<DataSheet> {
    return this.httpClient.get<DataSheet>(`${environment.apiUrl}/DataSheet/${productId}`);
  }
  public getDataSheetItem(id: number): Observable<DataSheetItem> {
    return this.httpClient.get<DataSheetItem>(`${environment.apiUrl}/DataSheet/${id}`);
  }
  public saveDataSheet(dataSheet: SaveDataSheet): Observable<number> {
    return this.httpClient.put<number>(`${environment.apiUrl}/DataSheet`, dataSheet);
  }
  public addDataSheetItem(item: NewDataSheetItem): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/DataSheet/Item`, item);
  }
  public updateDataSheetItem(item: UpdateDataSheetItem): Observable<any> {
    return this.httpClient.put<any>(`${environment.apiUrl}/DataSheet/Item`, item);
  }
  public deleteDataSheetItem(itemId: number): Observable<any> {
    return this.httpClient.delete<any>(`${environment.apiUrl}/DataSheet/Item/${itemId}`);
  }
  public calculateProduction(productId: number, measureId: number, quantity: number): Observable<ProductionSummary> {
    return this.httpClient.get<ProductionSummary>(`${environment.apiUrl}/DataSheet/Production/${productId}/${measureId}/${quantity}`);
  }
}
