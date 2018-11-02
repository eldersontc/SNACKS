import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IUsuario } from './usuario';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { UsuariosService } from './usuarios.service';

@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  usuarios: IUsuario[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IUsuario;

  columnas: string[][] = [
    ['L','Nombre'],
    ['L','Razón Social'],
    ['L','Nombres'],
    ['L','Apellidos']];
  atributos: string[][] = [
    ['S','L','nombre'],
    ['S','L','persona', 'razonSocial'],
    ['S','L','persona', 'nombres'],
    ['S','L','persona', 'apellidos']]

  constructor(private unidadService: UsuariosService) { }

  ngOnInit() {
    this.getUsuarios();
  }

  getUsuarios() {
    this.seleccion = undefined;
    this.unidadService.getUsuarios({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros.concat(this.extern || [])
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  seleccionar(e) {
    this.seleccion = e;
  }

  onGetSuccess(data: IListaRetorno<IUsuario>) {
    this.usuarios = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getUsuarios();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getUsuarios();
    }
  }

  deleteUsuario() {
    this.unidadService.deleteUsuario(this.seleccion.idUsuario).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getUsuarios();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }

}
