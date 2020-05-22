import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { Observable } from 'rxjs';
import { User } from '../../models/user';
import { environment } from 'src/environments/environment';
import { ParentModule } from '../../models/parentModule';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient) { }

  public GetUser(userId: number): Observable<User> {
    return this.httpClient.get<User>(`${environment.apiUrl}/User/${userId}`);
  }

  public GetPaged(page: number, pageSize: number, filter: string, active: boolean | null): Observable<User[]> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    if (active != null)
      params = params.append('active', String(active));
    return this.httpClient.get<User[]>(`${environment.apiUrl}/User/${page}/${pageSize}`, { params: params });
  }
  public Add(user: User): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/User`, user);
  }
  public Update(user: User): Observable<User> {
    return this.httpClient.put<User>(`${environment.apiUrl}/User`, user);
  }
  public GetUserModules(userId: number): Observable<ParentModule[]> {
    return this.httpClient.get<ParentModule[]>(`${environment.apiUrl}/User/Menus/${userId}`);
  }
}
