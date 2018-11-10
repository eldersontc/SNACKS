import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { ICaja } from './caja';

@Injectable({
  providedIn: 'root'
})
export class CajasService {

  private apiURL = this.baseUrl + 'api/cajas';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getCajas(params): Observable<IListaRetorno<ICaja>> {
    return this.http.post<IListaRetorno<ICaja>>(this.apiURL + '/GetCajas', params);
  }

  getAll(): Observable<ICaja[]> {
    return this.http.get<ICaja[]>(this.apiURL + '/GetAll');
  }

  getCaja(params): Observable<ICaja> {
    return this.http.get<ICaja>(this.apiURL + '/' + params);
  }

  createCaja(params: ICaja): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateCaja(params: ICaja): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idCaja, params);
  }

  deleteCaja(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
