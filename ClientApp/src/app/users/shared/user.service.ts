import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { User } from "./user.model";

@Injectable({
  providedIn: 'root'
})

export class UserComponentService {
  private apiUrl;

  constructor(private httpClient: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.apiUrl = apiUrl;
  }

  public getUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.getApiUrl());
  }

  public update(user): Observable<User> {
    const url = `${this.getApiUrl()}/${user.id}`;
    return this.httpClient
      .put<User>(url, user);
  }

  public delete(user: User): Observable<User> {
    const url = `${this.getApiUrl()}/${user.id}`;
    return this.httpClient
      .delete<User>(url);
  }

  public save(user: User): Observable<User> {
    return this.httpClient.post<User>(this.getApiUrl(), user);
  }

  public promote(user: User): Observable<User> {
    const url = `${this.getApiUrl()}/promote/${user.id}`;
    return this.httpClient
      .put<User>(url, user);
  }

  public demote(user: User): Observable<User> {
    const url = `${this.getApiUrl()}/demote/${user.id}`;
    return this.httpClient
      .put<User>(url, user);
  }

  public getCurrentUser(): Observable<User> {
    return this.httpClient.get<User>(this.getApiUrl() + '/current');
  }

  public getUser(id: string): Observable<User> {
    return this.getUsers()
      .pipe(
        map(users => users.find(user => user.id === id))
      );
  }

  private getApiUrl() {
    return this.apiUrl + 'users';
  }
}
