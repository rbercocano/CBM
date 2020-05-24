import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MeasureUnit } from '../../models/measureUnit';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CustomerType } from '../../models/customerType';
import { Role } from '../../models/role';
import { ContactType } from '../../models/contactType';
import { PaymentStatus } from '../../models/paymentStatus';
import { OrderStatus } from '../../models/orderStatus';
import { OrderItemStatus } from '../../models/orderItemStatus';

@Injectable({
  providedIn: 'root'
})
export class DomainService {

  constructor(private httpClient: HttpClient) { }

  public GetMeasureUnits(): Observable<MeasureUnit[]> {
    return this.httpClient.get<MeasureUnit[]>(`${environment.apiUrl}/Domain/MeasureUnit`);
  }
  public GetCustomerTypes(): Observable<CustomerType[]> {
    return this.httpClient.get<CustomerType[]>(`${environment.apiUrl}/Domain/CustomerType`);
  }
  public GetRoles(): Observable<Role[]> {
    return this.httpClient.get<Role[]>(`${environment.apiUrl}/Domain/Role`);
  }
  public GetContactTypes(): Observable<ContactType[]> {
    return this.httpClient.get<ContactType[]>(`${environment.apiUrl}/Domain/ContactType`);
  }
  public GetPaymentStatus(): Observable<PaymentStatus[]> {
    return this.httpClient.get<PaymentStatus[]>(`${environment.apiUrl}/Domain/PaymentStatus`);
  }
  public GetOrderStatus(): Observable<OrderStatus[]> {
    return this.httpClient.get<OrderStatus[]>(`${environment.apiUrl}/Domain/OrderStatus`);
  }
  public GetOrderItemStatus(): Observable<OrderItemStatus[]> {
    return this.httpClient.get<OrderItemStatus[]>(`${environment.apiUrl}/Domain/OrderItemStatus`);
  }
}
