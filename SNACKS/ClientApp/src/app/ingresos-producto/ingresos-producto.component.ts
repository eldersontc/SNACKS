import { Component, OnInit, Inject } from '@angular/core';
import { IIngresoProducto } from './ingreso-producto';
import { Filtro, IListaRetorno } from '../generico/generico';
import { IngresosProductoService } from './ingresos-producto.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-ingresos-producto',
  templateUrl: './ingresos-producto.component.html',
  styleUrls: ['./ingresos-producto.component.css']
})
export class IngresosProductoComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  ingresosProducto: IIngresoProducto[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: IIngresoProducto;
  rol: number;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private ingresoProductoService: IngresosProductoService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.rol = this.storage.get('login').tipo;
  }

  ngOnInit() {
    this.getIngresosProducto();
  }

  seleccionFecha() {
    this.quitarCriterio(this.criterio);
    this.filtros.push(new Filtro(this.criterio, '', 0, this.busqueda));
    this.getIngresosProducto();
  }

  getIngresosProducto() {
    this.ingresoProductoService.getIngresosProducto({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IIngresoProducto>) {
    this.ingresosProducto = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getIngresosProducto();
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getIngresosProducto();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteIngresoProducto(); } });
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
