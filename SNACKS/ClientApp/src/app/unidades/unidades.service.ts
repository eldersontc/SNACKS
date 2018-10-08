import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { IUnidad } from './unidad';
import { IListaRetorno } from '../generico/generico';

@Injectable()
export class UnidadesService {

  private apiURL = this.baseUrl + 'api/unidades';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getUnidades(params): Observable<IListaRetorno<IUnidad>> {
    return this.http.post<IListaRetorno<IUnidad>>(this.apiURL + '/GetUnidades', params);
  }

  createUnidad(params): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  } 
}
