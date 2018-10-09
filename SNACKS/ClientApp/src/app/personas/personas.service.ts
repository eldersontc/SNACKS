import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { IListaRetorno } from '../generico/generico';
import { IPersona } from './persona';

@Injectable()
export class PersonasService {

  private apiURL = this.baseUrl + 'api/personas';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getPersonas(params): Observable<IListaRetorno<IPersona>> {
    return this.http.post<IListaRetorno<IPersona>>(this.apiURL + '/GetPersonas', params);
  }

  getPersona(params): Observable<IPersona> {
    return this.http.get<IPersona>(this.apiURL + '/' + params);
  }

  createPersona(params: IPersona): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updatePersona(params: IPersona): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idPersona, params);
  }

  deletePersona(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
