import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Person } from '../../models/person';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Company } from '../../models/company';
import { PagedResult } from '../../models/pagedResult';
import { Contact } from '../../models/contact';
import { MergedCustomer } from '../../models/mergedCustomer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  constructor(private httpClient: HttpClient) { }

  public GetPerson(customerId: number): Observable<Person> {
    return this.httpClient.get<Person>(`${environment.apiUrl}/Customer/Person/${customerId}`);
  }

  public GetPagedPerson(page: number, pageSize: number, filter: string): Observable<PagedResult<Person>> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    return this.httpClient.get<PagedResult<Person>>(`${environment.apiUrl}/Customer/Person/${page}/${pageSize}`, { params: params });
  }
  public AddPerson(Customer: Person): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Customer/Person`, Customer);
  }
  public UpdatePerson(Customer: Person): Observable<Person> {
    return this.httpClient.put<Person>(`${environment.apiUrl}/Customer/Person`, Customer);
  }


  public GetCompany(customerId: number): Observable<Company> {
    return this.httpClient.get<Company>(`${environment.apiUrl}/Customer/Company/${customerId}`);
  }

  public GetPagedCompany(page: number, pageSize: number, filter: string): Observable<PagedResult<Company>> {
    let params = new HttpParams();
    if (filter)
      params = params.append('filter', filter);
    return this.httpClient.get<PagedResult<Company>>(`${environment.apiUrl}/Customer/Company/${page}/${pageSize}`, { params: params });
  }
  public AddCompany(Customer: Company): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Customer/Company`, Customer);
  }
  public UpdateCompany(Customer: Company): Observable<Company> {
    return this.httpClient.put<Company>(`${environment.apiUrl}/Customer/Company`, Customer);
  }

  public UpdateContact(contact: Contact): Observable<any> {
    return this.httpClient.put(`${environment.apiUrl}/Customer/Contact`, contact);
  }
  public AddContact(contact: Contact): Observable<number> {
    return this.httpClient.post<number>(`${environment.apiUrl}/Customer/Contact`, contact);
  }
  public DeleteContact(contactId: number): Observable<any> {
    return this.httpClient.delete(`${environment.apiUrl}/Customer/Contact/${contactId}`);
  }
  public GetContacts(customerId: number): Observable<Contact[]> {
    return this.httpClient.get<Contact[]>(`${environment.apiUrl}/Customer/Contact/${customerId}`);
  }
  public FilterMerged(filter: string): Observable<MergedCustomer[]> {
    return this.httpClient.get<MergedCustomer[]>(`${environment.apiUrl}/Customer/Merged/${filter}`);
  }
}
