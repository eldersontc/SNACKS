import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { IPedido } from './pedido';
import { PedidosService } from './pedidos.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IPersona } from '../personas/persona';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { IAlmacen } from '../almacenes/almacen';
import { ICaja } from '../cajas/caja';
import { AlmacenesService } from '../almacenes/almacenes.service';
import { CajasService } from '../cajas/cajas.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.css']
})
export class PedidosComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  fCliente: IFiltro[] = [];
  elegirCliente: boolean = false;

  pagina: number = 1;
  totalRegistros: number = 0;
  pedidos: IPedido[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IPedido;
  login: ILogin;

  almacenes: IAlmacen[] = [];
  cajas: ICaja[] = [];
  almacen: IAlmacen;
  caja: ICaja;
  importe: number;

  columnas: string[][] = [
    ['L', 'Nro. Pedido'],
    ['L', 'Creado Por'],
    ['L', 'Cliente'],
    ['L', 'Fecha Creación'],
    ['L', 'Estado'],];
  atributos: string[][] = [
    ['I', 'L', 'idPedido'],
    ['S', 'L', 'usuario', 'nombre'],
    ['S', 'L', 'cliente', 'razonSocial'],
    ['D', 'L', 'fechaCreacion'],
    ['S', 'L', 'estado']]

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    notifierService: NotifierService,
    private pedidoService: PedidosService,
    private almacenService: AlmacenesService,
    private cajaService: CajasService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.notifier = notifierService;
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.fCliente.push({
      k: 1,
      v: 'Cliente',
      n: 2
    });
    if (this.login.tipo == 2) {
      this.extern = [];
      this.extern.push({
        k: 3,
        v: this.login.nombrePersona,
        n: this.login.idPersona
      })
      this.criterio = 2;
    }
    if (this.login.tipo == 3) {
      this.extern = [];
      this.extern.push({
        k: 4,
        v: this.login.nombrePersona,
        n: this.login.idPersona
      });
      this.fCliente.push({
        k: 6,
        v: this.login.nombrePersona,
        n: this.login.idPersona
      });
    }
    this.getPedidos();
  }

  asignarCliente(e: IPersona) {
    this.elegirCliente = false;
    if (e) {
      this.filtros.push({ k: this.criterio, v: e.razonSocial, n: e.idPersona });
      this.getPedidos();
    }
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getPedidos();
  }

  getPedidos() {
    this.seleccion = undefined;
    this.pedidoService.getPedidos({
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

  onGetSuccess(data: IListaRetorno<IPedido>) {
    this.pedidos = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getPedidos();
  }

  buscar() {
    if (this.criterio == 1) {
      this.elegirCliente = true;
    }
  }

  openEntrega(content) {
    this.getAlmacenes();
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Entregar') { this.entregar(); } });
  }

  openPago(content) {
    this.getCajas();
    this.importe = this.seleccion.total - this.seleccion.pago;
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Pagar') { this.pagar(); } });
  }

  delete() {
    this.pedidoService.deletePedido(this.seleccion.idPedido)
      .subscribe(data => this.onProcessSuccess(),
      error => this.showError(error));
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onProcessSuccess() {
    this.seleccion = undefined;
    this.getPedidos();
  }

  getAlmacenes() {
    this.almacenService.getAll()
      .subscribe(d => this.onGetAlmacenesSuccess(d), error => console.error(error));
  }

  onGetAlmacenesSuccess(d) {
    this.almacenes = d;
    if (this.almacenes.length > 0) {
      this.almacen = this.almacenes[0];
    }
  }

  getCajas() {
    this.cajaService.getAll()
      .subscribe(d => this.onGetCajasSuccess(d), error => console.error(error));
  }

  onGetCajasSuccess(d) {
    this.cajas = d;
    if (this.cajas.length > 0) {
      this.caja = this.cajas[0];
    }
  }

  entregar() {
    this.pedidoService.delivery({
      idPedido: this.seleccion.idPedido,
      usuario: { idUsuario: this.login.id },
      almacen: this.almacen
    })
      .subscribe(data => this.onProcessSuccess(),
      error => this.showError(error));
  }

  pagar() {
    this.pedidoService.pay({
      idPedido: this.seleccion.idPedido,
      usuario: { idUsuario: this.login.id },
      idCaja: this.caja.idCaja,
      importe: this.importe
    })
      .subscribe(data => this.onProcessSuccess(),
      error => this.showError(error));
  }
}
