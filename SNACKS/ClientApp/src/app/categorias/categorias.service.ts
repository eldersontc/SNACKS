import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { ICategoria } from './categoria';

@Injectable({
  providedIn: 'root'
})
export class CategoriasService {

  private apiURL = this.baseUrl + 'api/categorias';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getCategorias(params): Observable<IListaRetorno<ICategoria>> {
    return this.http.post<IListaRetorno<ICategoria>>(this.apiURL + '/GetCategorias', params);
  }

  getCategoria(params): Observable<ICategoria> {
    return this.http.get<ICategoria>(this.apiURL + '/' + params);
  }

  createCategoria(params: ICategoria): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateCategoria(params: ICategoria): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idCategoria, params);
  }

  deleteCategoria(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
