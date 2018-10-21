import { Component, OnInit } from '@angular/core';
import { ISalidaInsumo } from './salida-insumo';
import { Filtro, IListaRetorno } from '../generico/generico';
import { SalidasInsumoService } from './salidas-insumo.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-salidas-insumo',
  templateUrl: './salidas-insumo.component.html',
  styleUrls: ['./salidas-insumo.component.css']
})
export class SalidasInsumoComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  salidasInsumo: ISalidaInsumo[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: ISalidaInsumo;

  constructor(private salidaInsumoService: SalidasInsumoService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getSalidasInsumo();
  }

  seleccionFecha() {
    this.quitarCriterio(this.criterio);
    this.filtros.push(new Filtro(this.criterio, '', 0, this.busqueda));
    this.getSalidasInsumo();
  }

  getSalidasInsumo() {
    this.salidaInsumoService.getSalidasInsumo({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<ISalidaInsumo>) {
    this.salidasInsumo = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getSalidasInsumo();
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getSalidasInsumo();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteSalidaInsumo(); } });
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
