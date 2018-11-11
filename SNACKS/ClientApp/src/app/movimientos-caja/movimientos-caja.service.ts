import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IMovimientoCaja } from './movimiento-caja';

@Injectable({
  providedIn: 'root'
})
export class MovimientosCajaService {

  private apiURL = this.baseUrl + 'api/movimientoscaja';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getMovimientosCaja(params): Observable<IListaRetorno<IMovimientoCaja>> {
    return this.http.post<IListaRetorno<IMovimientoCaja>>(this.apiURL + '/GetMovimientosCaja', params);
  }

  createMovimientoCaja(params: IMovimientoCaja): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }
}
