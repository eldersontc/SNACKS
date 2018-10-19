import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IProducto } from './producto';
import { Filtro, IListaRetorno } from '../generico/generico';
import { ProductosService } from './productos.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {

  @Input() params: Filtro;
  @Output() model = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  productos: IProducto[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  criterioCk: boolean = false;
  busqueda: string = '';
  seleccion: IProducto;

  constructor(private productoService: ProductosService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    if (this.params) {
      this.filtros.push(this.params);
    }
    this.getProductos();
  }

  getProductos() {
    this.productoService.getProductos({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IProducto>) {
    this.productos = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getProductos();
  }

  buscar() {
    if (this.criterio == 1) {
      if (this.busqueda.length > 0) {
        this.quitarCriterio(this.criterio);
        this.filtros.push(new Filtro(this.criterio, this.busqueda));
        this.busqueda = '';
        this.getProductos();
      }
    } else {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, 'Es Insumo : ' + ((this.criterioCk) ? 'Si' : 'No'), 0, new Date(), this.criterioCk));
      this.getProductos();
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
    this.getProductos();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteProducto(); } });
  }

  deleteProducto() {
    this.productoService.deleteProducto(this.seleccion.idProducto).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getProductos();
  }

  elegir() {
    if (this.params) {
      this.productoService.getProducto(this.seleccion.idProducto)
        .subscribe(producto => this.model.emit(producto),
          error => console.error(error));
    }
  }

  cancelar() {
    this.model.emit();
  }
}
