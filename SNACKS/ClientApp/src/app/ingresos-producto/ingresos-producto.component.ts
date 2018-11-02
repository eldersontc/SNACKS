import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { IIngresoProducto } from './ingreso-producto';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { IngresosProductoService } from './ingresos-producto.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-ingresos-producto',
  templateUrl: './ingresos-producto.component.html',
  styleUrls: ['./ingresos-producto.component.css']
})
export class IngresosProductoComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  ingresosProducto: IIngresoProducto[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IIngresoProducto;
  login: ILogin;

  columnas: string[][] = [
    ['L', 'Nro. Ingreso'],
    ['L', 'Creado Por'],
    ['L', 'Fecha Creación']];
  atributos: string[][] = [
    ['I', 'L', 'idIngresoProducto'],
    ['S', 'L', 'usuario', 'nombre'],
    ['D', 'L', 'fechaCreacion']]

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private ingresoProductoService: IngresosProductoService) {
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.getIngresosProducto();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getIngresosProducto();
  }

  getIngresosProducto() {
    this.seleccion = undefined;
    this.ingresoProductoService.getIngresosProducto({
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

  onGetSuccess(data: IListaRetorno<IIngresoProducto>) {
    this.ingresosProducto = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getIngresosProducto();
  }

  deleteIngresoProducto() {
    this.ingresoProductoService.deleteIngresoProducto(this.seleccion.idIngresoProducto).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getIngresosProducto();
  }

}
