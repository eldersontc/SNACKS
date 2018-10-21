import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { ISalidaProducto, IItemSalidaProducto } from './salida-producto';

@Injectable({
  providedIn: 'root'
})
export class SalidasProductoService {

  private apiURL = this.baseUrl + 'api/salidasproducto';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getSalidasProducto(params): Observable<IListaRetorno<ISalidaProducto>> {
    return this.http.post<IListaRetorno<ISalidaProducto>>(this.apiURL + '/GetSalidasProducto', params);
  }

  getSalidaProducto(params): Observable<ISalidaProducto> {
    return this.http.get<ISalidaProducto>(this.apiURL + '/' + params);
  }

  createSalidaProducto(params: ISalidaProducto): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateSalidaProducto(params: ISalidaProducto): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idSalidaProducto, params);
  }

  deleteSalidaProducto(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  createItem(params: IItemSalidaProducto): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/AddItem', params);
  }

  deleteItem(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/DeleteItem/' + params);
  }
}
