import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IListaRetorno } from '../generico/generico';
import { ILote, IItemLote } from './lote';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LotesService {

  private apiURL = this.baseUrl + 'api/lotes';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getLotes(params): Observable<IListaRetorno<ILote>> {
    return this.http.post<IListaRetorno<ILote>>(this.apiURL + '/GetLotes', params);
  }

  getItemsOnly(params): Observable<IItemLote[]> {
    return this.http.get<IItemLote[]>(this.apiURL + '/GetItemsOnly/' + params);
  }

  getItems(params): Observable<IItemLote[]> {
    return this.http.get<IItemLote[]>(this.apiURL + '/GetItems/' + params);
  }

  getLote(params): Observable<ILote> {
    return this.http.get<ILote>(this.apiURL + '/' + params);
  }

  createLote(params: ILote): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateLote(params: ILote): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idLote, params);
  }

  deleteLote(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
