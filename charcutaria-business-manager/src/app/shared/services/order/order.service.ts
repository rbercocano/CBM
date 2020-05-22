import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order } from '../../models/order';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { OrderDetails } from '../../models/orderDetails';

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
}
