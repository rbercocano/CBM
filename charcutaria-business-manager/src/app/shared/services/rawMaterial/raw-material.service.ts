import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RawMaterial } from '../../models/rawMaterial';
import { environment } from 'src/environments/environment';
import { PagedResult } from '../../models/pagedResult';

@Injectable({
  providedIn: 'root'
})
export class RawMaterialService {
  constructor(private httpClient: HttpClient) { }
  public Get(rawMaterialId: number): Observable<RawMaterial> {
    return this.httpClient.get<RawMaterial>(`${environment.apiUrl}/RawMaterial/${rawMaterialId}`);
  }
  public GetPaged(page: number, pageSize: number, name: string, sortDirection: number): Observable<PagedResult<RawMaterial>> {
    let params = new HttpParams();
    if (name)
      params = params.append('name', name);
    params = params.append('direction', String(sortDirection));
    return this.httpClient.get<PagedResult<RawMaterial>>(`${environment.apiUrl}/RawMaterial/${page}/${pageSize}`, { params: params });
  }
  public Add(rawMaterial: RawMaterial): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/RawMaterial`, rawMaterial);
  }
  public Update(rawMaterial: RawMaterial): Observable<RawMaterial> {
    return this.httpClient.put<RawMaterial>(`${environment.apiUrl}/RawMaterial`, rawMaterial);
  }
  public GetAll(sortDirection: number): Observable<RawMaterial[]> {
    let params = new HttpParams();
    params = params.append('direction', String(sortDirection));
    return this.httpClient.get<RawMaterial[]>(`${environment.apiUrl}/RawMaterial`);
  }
  
}
