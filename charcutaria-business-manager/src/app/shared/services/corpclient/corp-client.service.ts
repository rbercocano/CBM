import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CorpClient } from '../../models/corpClient';
import { environment } from 'src/environments/environment';
import { Observable, of } from 'rxjs';
import { ClientRegistration } from '../../models/clientRegistration';
import { flatMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CorpClientService {


  constructor(private httpClient: HttpClient) { }
  public GetCorpClient(corpClientId: number): Observable<CorpClient> {
    return this.httpClient.get<CorpClient>(`${environment.apiUrl}/CorpClient/${corpClientId}`)
      .pipe(flatMap(r => {
        r.socialIdentifier = r.customerTypeId == 1 ? r.cpf : r.cnpj;
        return of(r);
      }));
  }

  public GetPaged(page: number, pageSize: number, filter: string, active: boolean | null): Observable<CorpClient[]> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    if (active != null)
      params = params.append('active', String(active));
    return this.httpClient.get<CorpClient[]>(`${environment.apiUrl}/CorpClient/${page}/${pageSize}`, { params: params });
  }
  public Register(registrationData: ClientRegistration): Observable<CorpClient> {
    return this.httpClient.post<CorpClient>(`${environment.apiUrl}/CorpClient/Register`, registrationData);
  }
  public Update(corpClient: CorpClient): Observable<CorpClient> {
    return this.httpClient.put<CorpClient>(`${environment.apiUrl}/CorpClient`, corpClient);
  }
  public GetActives(): Observable<CorpClient[]> {
    return this.httpClient.get<CorpClient[]>(`${environment.apiUrl}/CorpClient/Actives`);
  }
}
