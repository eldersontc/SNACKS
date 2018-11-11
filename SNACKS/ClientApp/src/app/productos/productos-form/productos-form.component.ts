import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProductosService } from '../productos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IProducto, IItemProducto, IInsumoProducto } from '../producto';
import { IUnidad } from '../../unidades/unidad';
import { ICategoria } from '../../categorias/categoria';

@Component({
  selector: 'app-productos-form',
  templateUrl: './productos-form.component.html',
  styleUrls: ['./productos-form.component.css']
})
export class ProductosFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private productoService: ProductosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
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

  modoEdicion: boolean;
  elegirUnidad: boolean = false;
  elegirCategoria: boolean = false;
  elegirInsumo: boolean = false;

  form: FormGroup;
  formItem: FormGroup;
  formInsumo: FormGroup;
  
  items: IItemProducto[] = [];
  insumos: IInsumoProducto[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idProducto: 0,
      nombre: '',
      categoria: this.fb.group({
        idCategoria: 0,
        nombre: ''
      }),
      esInsumo: false
    });
    this.formItem = this.fb.group({
      unidad: this.fb.group({
        idUnidad: 0,
        nombre: ''
      }),
      factor: ''
    });
    this.formInsumo = this.fb.group({
      insumo: this.fb.group({
        idProducto: 0,
        nombre: ''
      })
    });
  }

  cargarFormulario(producto: IProducto) {
    this.form.patchValue({
      idProducto: producto.idProducto,
      nombre: producto.nombre,
      categoria: producto.categoria,
      esInsumo: producto.esInsumo
    });
    this.items = producto.items;
    this.insumos = producto.insumos;
  }

  save() {
    let producto: IProducto = Object.assign({}, this.form.value);

    producto.items = this.items;
    producto.insumos = this.insumos;

    if (this.modoEdicion) {
      this.productoService.updateProducto(producto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
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
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemProducto) {
    this.items.forEach((item, index) => {
      if (item.unidad.idUnidad === i.unidad.idUnidad)
        this.items.splice(index, 1);
    });
  }

  saveInsumo() {
    let i: IInsumoProducto = Object.assign({}, this.formInsumo.value);
    this.insumos.push(i);
    this.formInsumo.reset();
  }

  deleteInsumo(i: IInsumoProducto) {
    this.insumos.forEach((insumo, index) => {
      if (insumo.insumo.idProducto === i.insumo.idProducto)
        this.insumos.splice(index, 1);
    });
  }

  buscarUnidad() {
    this.elegirUnidad = true;
  }

  asignarUnidad(e: IUnidad) {
    this.elegirUnidad = false;
    if (e) {
      this.formItem.patchValue({
        unidad: e
      });
    }
  }

  buscarCategoria() {
    this.elegirCategoria = true;
  }

  asignarCategoria(e: ICategoria) {
    this.elegirCategoria = false;
    if (e) {
      this.form.patchValue({
        categoria: e
      });
    }
  }

  buscarInsumo() {
    this.elegirInsumo = true;
  }

  asignarInsumo(e: IProducto) {
    this.elegirInsumo = false;
    if (e) {
      this.formInsumo.patchValue({
        insumo: e
      });
    }
  }

}
