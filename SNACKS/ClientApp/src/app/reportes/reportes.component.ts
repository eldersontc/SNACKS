import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { IReporte } from './reporte';
import { IFiltro, IListaRetorno, IEstadistica } from '../generico/generico';
import { ReportesService } from './reportes.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes.component.html',
  styleUrls: ['./reportes.component.css']
})
export class ReportesComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  view: boolean = false;

  pagina: number = 1;
  totalRegistros: number = 0;
  reportes: IReporte[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IReporte;
  reporteView: IReporte;

  columnas: string[][] = [
    ['L','Nro.'],
    ['L','Título'],
    ['L','Tipo']];
  atributos: string[][] = [
    ['I','L','idReporte'],
    ['S','L','titulo'],
    ['S','L','tipoReporte']]

  constructor(private reporteService: ReportesService,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getReportes();
  }

  getReportes() {
    this.seleccion = undefined;
    this.reporteService.getReportes({
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
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getReportes();
    }
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
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
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
