import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { ISalidaInsumo, IItemSalidaInsumo } from './salida-insumo';

@Injectable({
  providedIn: 'root'
})
export class SalidasInsumoService {

  private apiURL = this.baseUrl + 'api/salidasinsumo';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getSalidasInsumo(params): Observable<IListaRetorno<ISalidaInsumo>> {
    return this.http.post<IListaRetorno<ISalidaInsumo>>(this.apiURL + '/GetSalidasInsumo', params);
  }

  getSalidaInsumo(params): Observable<ISalidaInsumo> {
    return this.http.get<ISalidaInsumo>(this.apiURL + '/' + params);
  }

  createSalidaInsumo(params: ISalidaInsumo): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateSalidaInsumo(params: ISalidaInsumo): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idSalidaInsumo, params);
  }

  deleteSalidaInsumo(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }

  createItem(params: IItemSalidaInsumo): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL + '/AddItem', params);
  }

  deleteItem(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/DeleteItem/' + params);
  }
}
