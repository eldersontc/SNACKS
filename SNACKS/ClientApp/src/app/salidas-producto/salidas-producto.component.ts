import { Component, OnInit, Inject } from '@angular/core';
import { ISalidaProducto } from './salida-producto';
import { Filtro, IListaRetorno } from '../generico/generico';
import { SalidasProductoService } from './salidas-producto.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-salidas-producto',
  templateUrl: './salidas-producto.component.html',
  styleUrls: ['./salidas-producto.component.css']
})
export class SalidasProductoComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  salidasProducto: ISalidaProducto[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: ISalidaProducto;
  rol: number;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private salidaProductoService: SalidasProductoService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
    this.rol = this.storage.get('login').tipo;
  }

  ngOnInit() {
    this.getSalidasProducto();
  }

  seleccionFecha() {
    this.quitarCriterio(this.criterio);
    this.filtros.push(new Filtro(this.criterio, '', 0, this.busqueda));
    this.getSalidasProducto();
  }

  getSalidasProducto() {
    this.salidaProductoService.getSalidasProducto({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<ISalidaProducto>) {
    this.salidasProducto = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getSalidasProducto();
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getSalidasProducto();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteSalidaProducto(); } });
  }

  deleteSalidaProducto() {
    this.salidaProductoService.deleteSalidaProducto(this.seleccion.idSalidaProducto).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getSalidasProducto();
  }

}
