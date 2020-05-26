import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Order } from '../../models/order';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OrderDetails } from '../../models/orderDetails';
import { UpdateOrder } from '../../models/updateOrder';
import { OrderItem } from '../../models/orderItem';
import { NewOrderItem } from '../../models/NewOrderItem';
import { OrderSummary } from '../../models/orderSummary';
import { orderSummaryFilter } from '../../models/orderSummaryFilter';
import { PagedResult } from '../../models/pagedResult';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient: HttpClient) { }

  public create(order: Order): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order`, order);
  }
  public get(orderId: number): Observable<OrderDetails> {
    return this.httpClient.get<OrderDetails>(`${environment.apiUrl}/Order/${orderId}`);
  }
  public getByNumber(orderNumber: number): Observable<OrderDetails> {
    return this.httpClient.get<OrderDetails>(`${environment.apiUrl}/Order/number/${orderNumber}`);
  }
  public update(model: UpdateOrder): Observable<OrderDetails> {
    return this.httpClient.put<OrderDetails>(`${environment.apiUrl}/Order`, model);
  }
  public cancel(orderNumber: number): Observable<any> {
    return this.httpClient.post<any>(`${environment.apiUrl}/Order/cancel/${orderNumber}`, {});
  }
  public restore(orderNumber: number): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order/restore/${orderNumber}`, {});
  }
  public removeOrderItem(orderId: number, orderItemId: number): Observable<any> {
    return this.httpClient.delete(`${environment.apiUrl}/Order/${orderId}/Item/${orderItemId}`);
  }
  public addOrderItem(orderItem: NewOrderItem): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order/Item`, orderItem);
  }
  public updateOrderItem(orderItem: NewOrderItem): Observable<any> {
    return this.httpClient.put<any>(`${environment.apiUrl}/Order/Item`, orderItem);
  }
  public getOrderSummary(filter: orderSummaryFilter, page: number, pageSize: number): Observable<PagedResult<OrderSummary>> {
    let params = new HttpParams();
    if (filter.customer)
      params = params.append('customer', filter.customer);
    if (filter.createdOnFrom != null)
      params = params.append('createdOnFrom', filter.createdOnFrom);
    if (filter.createdOnTo != null)
      params = params.append('createdOnTo', filter.createdOnTo);
    if (filter.paidOnFrom != null)
      params = params.append('paidOnFrom', filter.paidOnFrom);
    if (filter.paidOnTo != null)
      params = params.append('paidOnTo', filter.paidOnTo);
    if (filter.completeByFrom != null)
      params = params.append('completeByFrom', filter.completeByFrom);
    if (filter.completeByTo != null)
      params = params.append('completeByTo', filter.completeByTo);
    if (filter.paymentStatus != null)
      params = params.append('paymentStatus', String(filter.paymentStatus));
    if (filter.orderStatus != null)
      params = params.append('orderStatus', String(filter.orderStatus));
    if (filter.orderBy != null)
      params = params.append('orderBy', String(filter.orderBy));
    if (filter.direction != null)
      params = params.append('direction', String(filter.direction));
    return this.httpClient.get<PagedResult<OrderSummary>>(`${environment.apiUrl}/Order/${page}/${pageSize}`, { params: params });

  }
}
