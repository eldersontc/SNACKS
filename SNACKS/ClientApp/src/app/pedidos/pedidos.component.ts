import { Component, OnInit, Inject, Input, Output, EventEmitter } from '@angular/core';
import { IFiltro, IListaRetorno, ILogin } from '../generico/generico';
import { IPedido } from './pedido';
import { PedidosService } from './pedidos.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IPersona } from '../personas/persona';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.css']
})
export class PedidosComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  filtrosCliente: IFiltro[] = [];
  elegirCliente: boolean = false;

  pagina: number = 1;
  totalRegistros: number = 0;
  pedidos: IPedido[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IPedido;
  montoPago: number;
  login: ILogin;

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

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private pedidoService: PedidosService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.filtrosCliente.push({ k: 1, v: 'Cliente', n: 2 });
    if (this.login.tipo == 2) {
      this.extern = [];
      this.extern.push({ k: 3, v: this.login.nombrePersona, n: this.login.idPersona })
      this.criterio = 2;
    }
    if (this.login.tipo == 3) {
      this.extern = [];
      this.extern.push({ k: 4, v: this.login.nombrePersona, n: this.login.idPersona })
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

  openDelivery(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Entregar') { this.delivery(); } });
  }

  openPay(content) {
    this.montoPago = this.seleccion.total - this.seleccion.pago;
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Pagar') { this.pay(); } });
  }

  delete() {
    this.pedidoService.deletePedido(this.seleccion.idPedido).subscribe(data => this.onProcessSuccess(),
      error => console.log(error));
  }

  onProcessSuccess() {
    this.seleccion = undefined;
    this.getPedidos();
  }

  delivery() {
    this.pedidoService.delivery(this.seleccion.idPedido)
      .subscribe(data => this.onProcessSuccess(),
        error => console.log(error));
  }

  pay() {
    this.pedidoService.pay({ idPedido: this.seleccion.idPedido, pago: this.montoPago })
      .subscribe(data => this.onProcessSuccess(),
        error => console.log(error));
  }
}
