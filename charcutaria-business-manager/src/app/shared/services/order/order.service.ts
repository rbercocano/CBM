import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../../models/order';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OrderDetails } from '../../models/orderDetails';
import { UpdateOrder } from '../../models/updateOrder';
import { OrderItem } from '../../models/orderItem';
import { NewOrderItem } from '../../models/NewOrderItem';

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
}
