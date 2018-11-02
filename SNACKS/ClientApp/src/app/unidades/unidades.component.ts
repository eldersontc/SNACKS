import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { IUnidad } from './unidad';
import { UnidadesService } from './unidades.service';
import { IListaRetorno, IFiltro } from '../generico/generico';

@Component({
  selector: 'app-unidades',
  templateUrl: './unidades.component.html',
  styleUrls: ['./unidades.component.css']
})
export class UnidadesComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  unidades: IUnidad[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IUnidad;

  columnas: string[][] = [
    ['L', 'Nombre'],
    ['L', 'Abreviación']];
  atributos: string[][] = [
    ['S', 'L', 'nombre'],
    ['S', 'L', 'abreviacion']]

  constructor(private unidadService: UnidadesService) { }

  ngOnInit() {
    this.getUnidades();
  }

  getUnidades() {
    this.seleccion = undefined;
    this.unidadService.getUnidades({
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

  onGetSuccess(data: IListaRetorno<IUnidad>) {
    this.unidades = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getUnidades();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getUnidades();
    }
  }

  deleteUnidad() {
    this.unidadService.deleteUnidad(this.seleccion.idUnidad).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getUnidades();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }
}
