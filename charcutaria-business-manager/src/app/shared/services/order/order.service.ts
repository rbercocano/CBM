import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Order } from '../../models/order';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OrderDetails } from '../../models/orderDetails';
import { UpdateOrder } from '../../models/updateOrder';
import { NewOrderItem } from '../../models/NewOrderItem';
import { OrderSummary } from '../../models/orderSummary';
import { orderSummaryFilter } from '../../models/orderSummaryFilter';
import { PagedResult } from '../../models/pagedResult';
import { OrderItemReportFilter } from '../../models/OrderItemReportFilter';
import { OrderItemReport } from '../../models/orderItemReport';
import { OrderCountSummary } from '../../models/orderCountSummary';
import { ProfitSummary } from '../../models/profitSummary';
import { SalesSummary } from '../../models/salesSummary';
import { PendingPaymentsSummary } from '../../models/pendingPaymentSummary';
import { SalesPerMonth } from '../../models/salesPerMonth';
import { SummarizedProductionFilter } from '../../models/summarizedProductionFilter';
import { SummarizedOrderReport } from '../../models/summarizedOrderReport';
import { PayOrder } from '../../models/payOrder';
import { RefundPayment } from '../../models/refundPayment';
import { map } from 'rxjs/operators';
import * as moment from 'moment-timezone';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient: HttpClient) { }

  public create(order: Order): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order`, order);
  }
  public get(orderId: number): Observable<OrderDetails> {
    return this.httpClient.get<OrderDetails>(`${environment.apiUrl}/Order/${orderId}`).pipe(map(r => {
      var zone = moment.tz.guess();
      if (r.paidOn != null)
        r.paidOn = moment(r.paidOn).tz(zone).format();
      if (r.createdOn != null)
        r.createdOn = moment(r.createdOn).tz(zone).format();
      if (r.lastUpdated != null)
        r.lastUpdated = moment(r.lastUpdated).tz(zone).format();
      return r;
    }));
  }
  public getByNumber(orderNumber: number): Observable<OrderDetails> {
    return this.httpClient.get<OrderDetails>(`${environment.apiUrl}/Order/number/${orderNumber}`).pipe(map(r => {
      var zone = moment.tz.guess();
      if (r.paidOn != null)
        r.paidOn = moment(r.paidOn).tz(zone).format();
      if (r.createdOn != null)
        r.createdOn = moment(r.createdOn).tz(zone).format();
      if (r.lastUpdated != null)
        r.lastUpdated = moment(r.lastUpdated).tz(zone).format();
      return r;
    }));
  }
  public update(model: UpdateOrder): Observable<any> {
    return this.httpClient.put<OrderDetails>(`${environment.apiUrl}/Order`, model);
  }
  public addPayment(model: PayOrder): Observable<any> {
    return this.httpClient.post<OrderDetails>(`${environment.apiUrl}/Order/Payment`, model);
  }
  public refundPayment(model: RefundPayment): Observable<any> {
    return this.httpClient.post<OrderDetails>(`${environment.apiUrl}/Order/Refund`, model);
  }
  public cancel(orderNumber: number): Observable<any> {
    return this.httpClient.post<any>(`${environment.apiUrl}/Order/cancel/${orderNumber}`, {});
  }
  public restore(orderNumber: number): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order/restore/${orderNumber}`, {});
  }
  public close(orderNumber: number): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Order/Close/${orderNumber}`, {});
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
    (filter.paymentStatus ?? []).forEach(s => {
      params = params.append('paymentStatus', String(s.paymentStatusId));
    });
    (filter.orderStatus ?? []).forEach(s => {
      params = params.append('orderStatus', String(s.orderStatusId));
    });
    if (filter.orderBy != null)
      params = params.append('orderBy', String(filter.orderBy));
    if (filter.direction != null)
      params = params.append('direction', String(filter.direction));
    return this.httpClient.get<PagedResult<OrderSummary>>(`${environment.apiUrl}/Order/${page}/${pageSize}`, { params: params }).pipe(map(data => {
      var zone = moment.tz.guess();
      (data.data ?? []).forEach(r => {
        if (r.paidOn != null)
          r.paidOn = moment(r.paidOn).tz(zone).format();
        if (r.createdOn != null)
          r.createdOn = moment(r.createdOn).tz(zone).format();
      })
      return data;
    }));

  }
  public getOrderItemReport(filter: OrderItemReportFilter, page: number, pageSize: number): Observable<PagedResult<OrderItemReport>> {
    let params = new HttpParams();
    if (filter.orderNumber != null)
      params = params.append('orderNumber', String(filter.orderNumber));
    if (filter.customerId != null)
      params = params.append('customerId', String(filter.customerId));
    (filter.itemStatus ?? []).forEach(s => {
      params = params.append('itemStatus', String(s.orderItemStatusId));
    });
    (filter.products ?? []).forEach(s => {
      params = params.append('productId', String(s));
    });
    (filter.orderStatus ?? []).forEach(s => {
      params = params.append('orderStatus', String(s.orderStatusId));
    });
    if (filter.volumeUnitId != null)
      params = params.append('volumeUnitId', String(filter.volumeUnitId));
    if (filter.massUnitId != null)
      params = params.append('massUnitId', String(filter.massUnitId));
    if (filter.completeByFrom != null)
      params = params.append('completeByFrom', filter.completeByFrom);
    if (filter.completeByTo != null)
      params = params.append('completeByTo', filter.completeByTo);
    if (filter.customer)
      params = params.append('customer', filter.customer);
    if (filter.orderBy != null)
      params = params.append('orderBy', String(filter.orderBy));
    if (filter.direction != null)
      params = params.append('direction', String(filter.direction));
    return this.httpClient.get<PagedResult<OrderItemReport>>(`${environment.apiUrl}/Order/Report/Item/${page}/${pageSize}`, { params: params }).pipe(map(data => {
      var zone = moment.tz.guess();
      (data.data ?? []).forEach(r => {
        if (r.lastStatusDate != null)
          r.lastStatusDate = moment(r.lastStatusDate).tz(zone).format();
      });
      return data;
    }));

  }
  public getOrderCountSummary(): Observable<OrderCountSummary> {
    return this.httpClient.get<OrderCountSummary>(`${environment.apiUrl}/Order/Report/OrderCountSummary`);
  }
  public getProfitSummary(): Observable<ProfitSummary> {
    return this.httpClient.get<ProfitSummary>(`${environment.apiUrl}/Order/Report/ProfitSummary`);
  }
  public getSalesSummary(): Observable<SalesSummary> {
    return this.httpClient.get<SalesSummary>(`${environment.apiUrl}/Order/Report/SalesSummary`);
  }
  public getPendingPaymentsSummary(): Observable<PendingPaymentsSummary> {
    return this.httpClient.get<PendingPaymentsSummary>(`${environment.apiUrl}/Order/Report/PendingPaymentsSummary`);
  }

  public getSalesPerMonth(): Observable<SalesPerMonth[]> {
    return this.httpClient.get<SalesPerMonth[]>(`${environment.apiUrl}/Order/Report/SalesPerMonth`);
  }
  public getSummarizedReport(filter: SummarizedProductionFilter, page: number, pageSize: number): Observable<PagedResult<SummarizedOrderReport>> {
    let params = new HttpParams();
    (filter.itemStatus ?? []).forEach(s => {
      params = params.append('itemStatus', String(s.orderItemStatusId));
    });
    (filter.products ?? []).forEach(s => {
      params = params.append('products', String(s.productId));
    });
    if (filter.volumeUnitId != null)
      params = params.append('volumeUnitId', String(filter.volumeUnitId));
    if (filter.massUnitId != null)
      params = params.append('massUnitId', String(filter.massUnitId));
    if (filter.orderBy != null)
      params = params.append('orderBy', String(filter.orderBy));
    if (filter.direction != null)
      params = params.append('direction', String(filter.direction));
    return this.httpClient.get<PagedResult<SummarizedOrderReport>>(`${environment.apiUrl}/Order/Report/SummarizedProduction/${page}/${pageSize}`, { params: params });

  }
}
