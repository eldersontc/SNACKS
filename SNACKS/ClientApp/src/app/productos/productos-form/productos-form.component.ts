import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProductosService } from '../productos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IProducto, IItemProducto } from '../producto';
import { IUnidad } from '../../unidades/unidad';

@Component({
  selector: 'app-productos-form',
  templateUrl: './productos-form.component.html',
  styleUrls: ['./productos-form.component.css']
})
export class ProductosFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private productoService: ProductosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  elegirUnidad: boolean = false;
  form: FormGroup;
  formItem: FormGroup;
  
  items: IItemProducto[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idProducto: 0,
      nombre: '',
      esInsumo: false
    });
    this.formItem = this.fb.group({
      unidad: this.fb.group({
        idUnidad: 0,
        nombre: ''
      }),
      factor: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.productoService.getProducto(params["id"])
          .subscribe(producto => this.cargarFormulario(producto),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(producto: IProducto) {
    this.form.patchValue({
      idProducto: producto.idProducto,
      nombre: producto.nombre,
      esInsumo: producto.esInsumo
    });
    this.items = producto.items;
  }

  save() {
    let producto: IProducto = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.productoService.updateProducto(producto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      producto.items = this.items;
      this.productoService.createProducto(producto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/productos"]);
  }

  saveItem() {
    let i: IItemProducto = Object.assign({}, this.formItem.value);
    
    if (this.modoEdicion) {

      let producto: IProducto = Object.assign({}, this.form.value);
      i.producto = producto;

      this.productoService.createItem(i)
        .subscribe(data => this.onSaveItemSuccess(i),
        error => console.error(error));

    } else {
      this.onSaveItemSuccess(i);
    }
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemProducto) {
    if (this.modoEdicion) {
      this.productoService.deleteItem(i.idItemProducto)
        .subscribe(data => this.onDeleteItemSuccess(i),
        error => console.log(error));
    } else {
      this.onDeleteItemSuccess(i);
    }
  }

  onDeleteItemSuccess(i: IItemProducto) {
    this.items.forEach((item, index) => {
      if (item.idItemProducto === i.idItemProducto) this.items.splice(index, 1);
    });
  }
  
  buscarUnidad() {
    this.elegirUnidad = true;
  }

  asignarUnidad(event: IUnidad) {
    this.elegirUnidad = false;
    if (event) {
      this.formItem.patchValue({
        unidad: event
      });
    }
  }
}
