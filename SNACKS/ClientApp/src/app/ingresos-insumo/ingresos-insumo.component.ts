import { Component, OnInit, Inject } from '@angular/core';
import { IIngresoInsumo } from './ingreso-insumo';
import { Filtro, IListaRetorno } from '../generico/generico';
import { IngresosInsumoService } from '../ingresos-insumo/ingresos-insumo.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';

@Component({
  selector: 'app-ingresos-insumo',
  templateUrl: './ingresos-insumo.component.html',
  styleUrls: ['./ingresos-insumo.component.css']
})
export class IngresosInsumoComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  ingresosInsumo: IIngresoInsumo[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IIngresoInsumo;
  rol: number;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private ingresoInsumoService: IngresosInsumoService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.rol = this.storage.get('login').tipo;
  }

  ngOnInit() {
    this.getIngresosInsumo();
  }

  seleccionFecha() {
    this.quitarCriterio(this.criterio);
    this.filtros.push(new Filtro(this.criterio, '', 0, this.busqueda));
    this.getIngresosInsumo();
  }

  getIngresosInsumo() {
    this.ingresoInsumoService.getIngresosInsumo({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IIngresoInsumo>) {
    this.ingresosInsumo = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getIngresosInsumo();
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getIngresosInsumo();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteIngresoInsumo(); } });
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
