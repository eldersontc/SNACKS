import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { IIngresoInsumo } from './ingreso-insumo';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { IngresosInsumoService } from '../ingresos-insumo/ingresos-insumo.service';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';

@Component({
  selector: 'app-ingresos-insumo',
  templateUrl: './ingresos-insumo.component.html',
  styleUrls: ['./ingresos-insumo.component.css']
})
export class IngresosInsumoComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  ingresosInsumo: IIngresoInsumo[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IIngresoInsumo;
  login: ILogin;

  columnas: string[][] = [
    ['L','Nro. Ingreso'],
    ['L','Creado Por'],
    ['L','Fecha Creación'],
    ['L','Costo']];
  atributos: string[][] = [
    ['I','L','idIngresoInsumo'],
    ['S','L','usuario','nombre'],
    ['D','L','fechaCreacion'],
    ['I','L','costo']]

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private ingresoInsumoService: IngresosInsumoService) {
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.getIngresosInsumo();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getIngresosInsumo();
  }

  getIngresosInsumo() {
    this.seleccion = undefined;
    this.ingresoInsumoService.getIngresosInsumo({
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

  onGetSuccess(data: IListaRetorno<IIngresoInsumo>) {
    this.ingresosInsumo = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getIngresosInsumo();
  }

  deleteIngresoInsumo() {
    this.ingresoInsumoService.deleteIngresoInsumo(this.seleccion.idIngresoInsumo).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getIngresosInsumo();
  }

}
