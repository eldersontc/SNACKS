import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IIngresoProducto, IItemIngresoProducto } from './ingreso-producto';

@Injectable({
  providedIn: 'root'
})
export class IngresosProductoService {

  private apiURL = this.baseUrl + 'api/ingresosproducto';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getIngresosProducto(params): Observable<IListaRetorno<IIngresoProducto>> {
    return this.http.post<IListaRetorno<IIngresoProducto>>(this.apiURL + '/GetIngresosProducto', params);
  }

  getIngresoProducto(params): Observable<IIngresoProducto> {
    return this.http.get<IIngresoProducto>(this.apiURL + '/' + params);
  }

  createIngresoProducto(params: IIngresoProducto): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateIngresoProducto(params: IIngresoProducto): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idIngresoProducto, params);
  }

  deleteIngresoProducto(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  createItem(params: IItemIngresoProducto): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/AddItem', params);
  }

  deleteItem(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/DeleteItem/' + params);
  }
}
