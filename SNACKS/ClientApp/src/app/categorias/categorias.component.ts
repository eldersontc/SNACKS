import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ICategoria } from './categoria';
import { Filtro, IListaRetorno } from '../generico/generico';
import { CategoriasService } from './categorias.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-categorias',
  templateUrl: './categorias.component.html',
  styleUrls: ['./categorias.component.css']
})
export class CategoriasComponent implements OnInit {

  @Input() modo: number;
  @Output() model = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  categorias: ICategoria[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: ICategoria;

  constructor(private categoriaService: CategoriasService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getCategorias();
  }

  getCategorias() {
    this.categoriaService.getCategorias({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
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
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, this.busqueda));
      this.busqueda = '';
      this.getCategorias();
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
    this.getCategorias();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteCategoria(); } });
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
    this.model.emit(this.seleccion);
  }

  cancelar() {
    this.model.emit();
  }
}
