import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IPedido, IItemPedido } from './pedido';

@Injectable()
export class PedidosService {

  private apiURL = this.baseUrl + 'api/pedidos';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getPedidos(params): Observable<IListaRetorno<IPedido>> {
    return this.http.post<IListaRetorno<IPedido>>(this.apiURL + '/GetPedidos', params);
  }

  getPedido(params): Observable<IPedido> {
    return this.http.get<IPedido>(this.apiURL + '/' + params);
  }

  createPedido(params: IPedido): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updatePedido(params: IPedido): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idPedido, params);
  }

  deletePedido(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  delivery(params): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/Entregar/' + params, params);
  }

  pay(params): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/Pagar/' + params.idPedido, params.pago);
  }
}
