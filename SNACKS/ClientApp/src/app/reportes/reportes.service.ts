import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno, IEstadistica } from '../generico/generico';
import { IReporte, IItemReporte } from './reporte';

@Injectable({
  providedIn: 'root'
})
export class ReportesService {

  private apiURL = this.baseUrl + 'api/reportes';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getReportes(params): Observable<IListaRetorno<IReporte>> {
    return this.http.post<IListaRetorno<IReporte>>(this.apiURL + '/GetReportes', params);
  }

  runReporte(params): Observable<IEstadistica[]> {
    return this.http.post<IEstadistica[]>(this.apiURL + '/RunReporte', params);
  }

  getReporte(params): Observable<IReporte> {
    return this.http.get<IReporte>(this.apiURL + '/' + params);
  }

  createReporte(params: IReporte): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateReporte(params: IReporte): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idReporte, params);
  }

  deleteReporte(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  createItem(params: IItemReporte): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/AddItem', params);
  }

  deleteItem(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/DeleteItem/' + params);
  }
}
