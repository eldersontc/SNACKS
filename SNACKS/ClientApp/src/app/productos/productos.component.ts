import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IProducto } from './producto';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { ProductosService } from './productos.service';

@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  productos: IProducto[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  criterioCk: boolean = false;
  busqueda: string = '';
  seleccion: IProducto;

  columnas: string[][] = [
    ['L', 'Nombre'],
    ['L', 'Categoria'],
    ['C', 'Es Insumo']];
  atributos: string[][] = [
    ['S', 'L', 'nombre'],
    ['S', 'L', 'categoria', 'nombre'],
    ['B', 'C', 'esInsumo']]

  constructor(private productoService: ProductosService) { }

  ngOnInit() {
    this.getProductos();
  }

  getProductos() {
    this.seleccion = undefined;
    this.productoService.getProductos({
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
        this.filtros.push({ k: this.criterio, v: this.busqueda });
        this.busqueda = '';
        this.getProductos();
      }
    } else {
      this.filtros.push({ k: this.criterio, v: 'Es Insumo : ' + ((this.criterioCk) ? 'Si' : 'No'), b: this.criterioCk });
      this.getProductos();
    }
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
    if (this.extern) {
      this.productoService.getProducto(this.seleccion.idProducto)
        .subscribe(producto => this.select.emit(producto),
          error => console.error(error));
    }
  }

  cancelar() {
    this.select.emit();
  }
}
