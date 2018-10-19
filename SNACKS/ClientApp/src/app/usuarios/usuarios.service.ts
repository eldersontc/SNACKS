import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IListaRetorno } from '../generico/generico';
import { IUsuario } from './usuario';

@Injectable({
  providedIn: 'root'
})
export class UsuariosService {

  private apiURL = this.baseUrl + 'api/usuarios';

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getUsuarios(params): Observable<IListaRetorno<IUsuario>> {
    return this.http.post<IListaRetorno<IUsuario>>(this.apiURL + '/GetUsuarios', params);
  }

  getUsuario(params): Observable<IUsuario> {
    return this.http.get<IUsuario>(this.apiURL + '/' + params);
  }

  createUsuario(params: IUsuario): Observable<boolean> {
    return this.http.post<boolean>(this.apiURL, params);
  }

  updateUsuario(params: IUsuario): Observable<boolean> {
    return this.http.put<boolean>(this.apiURL + '/' + params.idUsuario, params);
  }

  deleteUsuario(params): Observable<boolean> {
    return this.http.delete<boolean>(this.apiURL + '/' + params);
  }
}
