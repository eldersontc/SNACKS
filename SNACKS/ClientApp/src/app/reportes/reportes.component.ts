import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { IReporte } from './reporte';
import { Filtro, IListaRetorno } from '../generico/generico';
import { ReportesService } from './reportes.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {

  @Input() include: boolean = false;
  @Output() model = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  reportes: IReporte[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IReporte;

  constructor(private reporteService: ReportesService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getReportes();
  }

  getReportes() {
    this.reporteService.getReportes({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IReporte>) {
    this.reportes = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getReportes();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, this.busqueda));
      this.busqueda = '';
      this.getReportes();
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
    this.getReportes();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteReporte(); } });
  }

  deleteReporte() {
    this.reporteService.deleteReporte(this.seleccion.idReporte).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getReportes();
  }

  elegir() {
    this.model.emit(this.seleccion);
  }

  cancelar() {
    this.model.emit();
  }

}
