import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IProducto } from './producto';

@Injectable()
export class ProductosService {

  private apiURL = this.baseUrl + 'api/productos';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getProductos(params): Observable<IListaRetorno<IProducto>> {
    return this.http.post<IListaRetorno<IProducto>>(this.apiURL + '/GetProductos', params);
  }

  getProducto(params): Observable<IProducto> {
    return this.http.get<IProducto>(this.apiURL + '/' + params);
  }

  createProducto(params: IProducto): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateProducto(params: IProducto): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idProducto, params);
  }

  deleteProducto(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
