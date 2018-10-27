import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IZonaVenta } from './zonaVenta';

@Injectable({
  providedIn: 'root'
})
export class ZonasVentaService {

  private apiURL = this.baseUrl + 'api/zonasVenta';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getZonasVenta(params): Observable<IListaRetorno<IZonaVenta>> {
    return this.http.post<IListaRetorno<IZonaVenta>>(this.apiURL + '/GetZonasVenta', params);
  }

  getZonaVenta(params): Observable<IZonaVenta> {
    return this.http.get<IZonaVenta>(this.apiURL + '/' + params);
  }

  createZonaVenta(params: IZonaVenta): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateZonaVenta(params: IZonaVenta): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idZonaVenta, params);
  }

  deleteZonaVenta(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
