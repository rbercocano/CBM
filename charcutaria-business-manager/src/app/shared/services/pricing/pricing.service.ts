import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PricingRequest } from '../../models/pricingRequest';

@Injectable({
  providedIn: 'root'
})
export class PricingService {

  constructor(private httpClient: HttpClient) { }
  public calculatePrice(pricingRequest: PricingRequest): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Pricing`, pricingRequest);
  }
}
