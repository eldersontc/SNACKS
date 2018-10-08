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

  getUnidad(params): Observable<IUnidad> {
    return this.http.get<IUnidad>(this.apiURL + '/' + params);
  }

  createUnidad(params: IUnidad): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateUnidad(params: IUnidad): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idUnidad, params);
  }

  deleteUnidad(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
