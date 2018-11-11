import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { ISalidaInsumo } from './salida-insumo';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { SalidasInsumoService } from './salidas-insumo.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-salidas-insumo',
  templateUrl: './salidas-insumo.component.html',
  styleUrls: ['./salidas-insumo.component.css']
})
export class SalidasInsumoComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  salidasInsumo: ISalidaInsumo[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: ISalidaInsumo;
  login: ILogin;

  columnas: string[][] = [
    ['L', 'Nro. Salida'],
    ['L', 'Lote'],
    ['L', 'Creado Por'],
    ['L', 'Fecha Creación']];
  atributos: string[][] = [
    ['I', 'L', 'idSalidaInsumo'],
    ['I', 'L', 'idLote'],
    ['S', 'L', 'usuario', 'nombre'],
    ['D', 'L', 'fechaCreacion']]

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private salidaInsumoService: SalidasInsumoService) {
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.getSalidasInsumo();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getSalidasInsumo();
  }

  getSalidasInsumo() {
    this.seleccion = undefined;
    this.salidaInsumoService.getSalidasInsumo({
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

  onGetSuccess(data: IListaRetorno<ISalidaInsumo>) {
    this.salidasInsumo = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getSalidasInsumo();
  }

  deleteSalidaInsumo() {
    this.salidaInsumoService.deleteSalidaInsumo(this.seleccion.idSalidaInsumo).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getSalidasInsumo();
  }

}
