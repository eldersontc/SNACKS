import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IZonaVenta } from './zonaVenta';
import { Filtro, IListaRetorno } from '../generico/generico';
import { ZonasVentaService } from './zonas-venta.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-zonas-venta',
  templateUrl: './zonas-venta.component.html',
  styleUrls: ['./zonas-venta.component.css']
})
export class ZonasVentaComponent implements OnInit {

  @Input() modo: number;
  @Output() model = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  zonasVenta: IZonaVenta[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IZonaVenta;

  constructor(private zonaVentaService: ZonasVentaService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getZonasVenta();
  }

  getZonasVenta() {
    this.zonaVentaService.getZonasVenta({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IZonaVenta>) {
    this.zonasVenta = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getZonasVenta();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, this.busqueda));
      this.busqueda = '';
      this.getZonasVenta();
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
    this.getZonasVenta();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteZonaVenta(); } });
  }

  deleteZonaVenta() {
    this.zonaVentaService.deleteZonaVenta(this.seleccion.idZonaVenta).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getZonasVenta();
  }

  elegir() {
    this.model.emit(this.seleccion);
  }

  cancelar() {
    this.model.emit();
  }

}
