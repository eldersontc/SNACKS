import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { IAlmacen } from './almacen';
import { AlmacenesService } from './almacenes.service';

@Component({
  selector: 'app-almacenes',
  templateUrl: './almacenes.component.html',
  styleUrls: ['./almacenes.component.css']
})
export class AlmacenesComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  almacenes: IAlmacen[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IAlmacen;

  columnas: string[][] = [
    ['L', 'Nombre']];
  atributos: string[][] = [
    ['S', 'L', 'nombre']]

  constructor(private almacenService: AlmacenesService) { }

  ngOnInit() {
    this.getAlmacenes();
  }

  getAlmacenes() {
    this.seleccion = undefined;
    this.almacenService.getAlmacenes({
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

  onGetSuccess(data: IListaRetorno<IAlmacen>) {
    this.almacenes = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getAlmacenes();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getAlmacenes();
    }
  }

  deleteAlmacen() {
    this.almacenService.deleteAlmacen(this.seleccion.idAlmacen).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getAlmacenes();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }

}
