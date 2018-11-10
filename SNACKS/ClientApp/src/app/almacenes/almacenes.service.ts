import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IAlmacen } from './almacen';

@Injectable({
  providedIn: 'root'
})
export class AlmacenesService {

  private apiURL = this.baseUrl + 'api/almacenes';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAlmacenes(params): Observable<IListaRetorno<IAlmacen>> {
    return this.http.post<IListaRetorno<IAlmacen>>(this.apiURL + '/GetAlmacenes', params);
  }

  getAll(): Observable<IAlmacen[]> {
    return this.http.get<IAlmacen[]>(this.apiURL + '/GetAll');
  }

  getAlmacen(params): Observable<IAlmacen> {
    return this.http.get<IAlmacen>(this.apiURL + '/' + params);
  }

  createAlmacen(params: IAlmacen): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateAlmacen(params: IAlmacen): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idAlmacen, params);
  }

  deleteAlmacen(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
