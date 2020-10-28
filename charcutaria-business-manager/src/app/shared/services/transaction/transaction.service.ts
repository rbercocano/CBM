import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Balance } from '../../models/balance';
import { NewTransaction } from '../../models/newTransaction';
import * as moment from 'moment-timezone';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private httpClient: HttpClient) { }
  public GetBalance(start: string, end: string): Observable<Balance[]> {
    let params = new HttpParams();
    params = params.append('start', start);
    params = params.append('end', end);
    return this.httpClient.get<Balance[]>(`${environment.apiUrl}/Transaction/Balance`, { params: params });
  }
  public GetTotalBalance(): Observable<number> {
    return this.httpClient.get<number>(`${environment.apiUrl}/Transaction/Balance/Total`);
  }
  public Add(transaction: NewTransaction): Observable<Balance[]> {
    transaction.date = moment(transaction.date).format();
    return this.httpClient.post<Balance[]>(`${environment.apiUrl}/Transaction`, transaction);
  }
}
