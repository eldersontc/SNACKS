import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICategoria } from './categoria';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { CategoriasService } from './categorias.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-categorias',
  templateUrl: './categorias.component.html',
  styleUrls: ['./categorias.component.css']
})
export class CategoriasComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  categorias: ICategoria[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: ICategoria;

  columnas: string[][] = [
    ['L', 'Nombre']];
  atributos: string[][] = [
    ['S', 'L', 'nombre']]

  constructor(private categoriaService: CategoriasService) { }

  ngOnInit() {
    this.getCategorias();
  }

  getCategorias() {
    this.seleccion = undefined;
    this.categoriaService.getCategorias({
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

  onGetSuccess(data: IListaRetorno<ICategoria>) {
    this.categorias = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getCategorias();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getCategorias();
    }
  }

  deleteCategoria() {
    this.categoriaService.deleteCategoria(this.seleccion.idCategoria).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getCategorias();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }
}
