import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CorpClient } from '../../models/corpClient';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CorpClientService {


  constructor(private httpClient: HttpClient) { }
  public GetCorpClient(corpClientId: number): Observable<CorpClient> {
    return this.httpClient.get<CorpClient>(`${environment.apiUrl}/CorpClient/${corpClientId}`);
  }

  public GetPaged(page: number, pageSize: number, filter: string, active: boolean | null): Observable<CorpClient[]> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    if (active != null)
      params = params.append('active', String(active));
    return this.httpClient.get<CorpClient[]>(`${environment.apiUrl}/CorpClient/${page}/${pageSize}`, { params: params });
  }
  public Add(CorpClient: CorpClient): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/CorpClient`, CorpClient);
  }
  public Update(CorpClient: CorpClient): Observable<CorpClient> {
    return this.httpClient.put<CorpClient>(`${environment.apiUrl}/CorpClient`, CorpClient);
  }
  public GetActives(): Observable<CorpClient[]> {
    return this.httpClient.get<CorpClient[]>(`${environment.apiUrl}/CorpClient/Actives`);
  }
}
