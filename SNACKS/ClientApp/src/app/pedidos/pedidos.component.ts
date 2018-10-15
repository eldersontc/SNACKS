import { Component, OnInit } from '@angular/core';
import { Filtro, IListaRetorno, DatepickerI18n, DateParserFormatter, DateAdapter } from '../generico/generico';
import { IPedido } from './pedido';
import { PedidosService } from './pedidos.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { IPersona } from '../personas/persona';

@Component({
  selector: 'app-pedidos',
  templateUrl: './pedidos.component.html',
  styleUrls: ['./pedidos.component.css']
})
export class PedidosComponent implements OnInit {

  filtroCliente: Filtro = new Filtro(1, 'Cliente', 2);
  elegirCliente: boolean = false;

  pagina: number = 1;
  totalRegistros: number = 0;
  pedidos: IPedido[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IPedido;

  constructor(private pedidoService: PedidosService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getPedidos();
  }

  asignarCliente(event: IPersona) {
    this.elegirCliente = false;
    if (event) {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, event.razonSocial, event.idPersona));
      this.getPedidos();
    }
  }

  seleccionFecha() {
    this.quitarCriterio(this.criterio);
    this.filtros.push(new Filtro(this.criterio, '', 0, this.busqueda));
    this.getPedidos();
  }

  getPedidos() {
    this.pedidoService.getPedidos({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
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

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getPedidos();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deletePedido(); } });
  }

  deletePedido() {
    this.pedidoService.deletePedido(this.seleccion.idPedido).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getPedidos();
  }

}
