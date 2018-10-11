import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProductosService } from '../productos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IProducto } from '../producto';

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
  formGroup: FormGroup;

  ngOnInit() {
    this.formGroup = this.fb.group({
      idProducto: 0,
      nombre: '',
      esInsumo: false
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.productoService.getProducto(params["id"]).subscribe(producto => this.cargarFormulario(producto),
          error => console.error(error));
      }
    });
  }

  get f() { return this.formGroup.controls; }

  cargarFormulario(producto: IProducto) {
    this.formGroup.patchValue({
      idProducto: producto.idProducto,
      nombre: producto.nombre,
      esInsumo: producto.esInsumo
    });
  }

  save() {
    let producto: IProducto = Object.assign({}, this.formGroup.value);

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
}
