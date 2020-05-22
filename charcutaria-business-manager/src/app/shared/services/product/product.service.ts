import { Injectable } from '@angular/core';
import { Product } from '../../models/product';
import { HttpParams, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PagedResult } from '../../models/pagedResult';

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
}
