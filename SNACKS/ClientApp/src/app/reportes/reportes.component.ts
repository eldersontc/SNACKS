import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { IReporte } from './reporte';
import { Filtro, IListaRetorno, IEstadistica } from '../generico/generico';
import { ReportesService } from './reportes.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {

  @Input() include: boolean = false;
  @Output() model = new EventEmitter();

  view: boolean = false;

  pagina: number = 1;
  totalRegistros: number = 0;
  reportes: IReporte[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IReporte;
  reporteView: IReporte;

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

  getReporte(content) {
    this.reporteService.getReporte(this.seleccion.idReporte)
      .subscribe(data => this.openViewer(content, data),
        error => console.log(error));
  }

  openViewer(content, data) {
    this.reporteView = data
    this.modalService.open(content, { centered: true, size: 'lg' })
      .result.then((result) => { this.view = false });
  }

  chart = [];

  dynamicColor() {
    var r = Math.floor(Math.random() * 255);
    var g = Math.floor(Math.random() * 255);
    var b = Math.floor(Math.random() * 255);
    return "rgb(" + r + "," + g + "," + b + ")";
  };

  groupBy(xs, key) {
    return xs.reduce(function (rv, x) {
      (rv[x[key]] = rv[x[key]] || []).push(x);
      return rv;
    }, {});
  }

  runReporte() {
    this.view = true;
    this.reporteService.runReporte(this.reporteView)
      .subscribe(data => this.makeChart(data),
        error => console.error(error));
  }

  makeChart(estadisticas: IEstadistica[]) {

    var leyendas = Object.keys(this.groupBy(estadisticas, 'leyenda'));
    var etiquetas = Object.keys(this.groupBy(estadisticas, 'etiqueta'));

    var data = [];
    var colors = [];
    var datasets = [];

    leyendas.forEach((l) => {

      data = [];
      colors = [];

      etiquetas.forEach((e) => {
        var v = estadisticas.filter(obj => {
          return obj.leyenda == l && obj.etiqueta == e
        })
        data.push(v.length == 0 ? 0 : v[0].valor);
        colors.push(this.dynamicColor());
      });

      datasets.push({
        label: l,
        data: data,
        backgroundColor: leyendas.length > 1
          ? this.dynamicColor() : colors
      });
    });

    this.chart = new Chart('canvas', {
      type: this.seleccion.tipoReporte,
      data: {
        labels: etiquetas,
        datasets: datasets
      },
      options: {
        title: {
          text: this.seleccion.titulo,
          display: true
        },
        responsive: true
      }
    })
  }

}
