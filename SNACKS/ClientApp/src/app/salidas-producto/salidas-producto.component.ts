import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { ISalidaProducto } from './salida-producto';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { SalidasProductoService } from './salidas-producto.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-salidas-producto',
  templateUrl: './salidas-producto.component.html',
  styleUrls: ['./salidas-producto.component.css']
})
export class SalidasProductoComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  salidasProducto: ISalidaProducto[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: ISalidaProducto;
  login: ILogin;

  columnas: string[][] = [
    ['L', 'N° Salida'],
    ['L', 'Creado Por'],
    ['L', 'Fecha Creación'],
    ['L', 'N° Pedido'],];
  atributos: string[][] = [
    ['I', 'L', 'idSalidaProducto'],
    ['S', 'L', 'usuario', 'nombre'],
    ['D', 'L', 'fechaCreacion'],
    ['I', 'L', 'idPedido']]

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    notifierService: NotifierService,
    private salidaProductoService: SalidasProductoService) {
    this.notifier = notifierService;
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.getSalidasProducto();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getSalidasProducto();
  }

  getSalidasProducto() {
    this.seleccion = undefined;
    this.salidaProductoService.getSalidasProducto({
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

  onGetSuccess(data: IListaRetorno<ISalidaProducto>) {
    this.salidasProducto = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getSalidasProducto();
  }

  deleteSalidaProducto() {
    this.salidaProductoService
      .deleteSalidaProducto(this.seleccion.idSalidaProducto)
      .subscribe(data => this.onDeleteSuccess(),
      error => this.showError(error));
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getSalidasProducto();
  }

}
