import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IIngresoInsumo, IItemIngresoInsumo } from './ingreso-insumo';

@Injectable({
  providedIn: 'root'
})
export class IngresosInsumoService {

  private apiURL = this.baseUrl + 'api/ingresosinsumo';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getIngresosInsumo(params): Observable<IListaRetorno<IIngresoInsumo>> {
    return this.http.post<IListaRetorno<IIngresoInsumo>>(this.apiURL + '/GetIngresosInsumo', params);
  }

  getIngresoInsumo(params): Observable<IIngresoInsumo> {
    return this.http.get<IIngresoInsumo>(this.apiURL + '/' + params);
  }

  createIngresoInsumo(params: IIngresoInsumo): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateIngresoInsumo(params: IIngresoInsumo): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idIngresoInsumo, params);
  }

  deleteIngresoInsumo(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  createItem(params: IItemIngresoInsumo): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/AddItem', params);
  }

  deleteItem(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/DeleteItem/' + params);
  }
}
