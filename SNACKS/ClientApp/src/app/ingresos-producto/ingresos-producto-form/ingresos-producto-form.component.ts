import { Component, OnInit, Inject } from '@angular/core';
import { Filtro } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IngresosProductoService } from '../ingresos-producto.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemIngresoProducto, IIngresoProducto } from '../ingreso-producto';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';

@Component({
  selector: 'app-ingresos-producto-form',
  templateUrl: './ingresos-producto-form.component.html',
  styleUrls: ['./ingresos-producto-form.component.css']
})
export class IngresosProductoFormComponent implements OnInit {

  filtroProducto: Filtro = new Filtro(2, 'Producto', 0, new Date(), false);
  elegirProducto: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private ingresoProductoService: IngresosProductoService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemIngresoProducto[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idIngresoProducto: 0,
      fechaCreacion: new Date(),
      comentario: ''
    });
    this.formItem = this.fb.group({
      producto: this.fb.group({
        idProducto: 0,
        nombre: '',
        items: []
      }),
      unidad: '',
      cantidad: '',
      factor: 0
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.ingresoProductoService.getIngresoProducto(params["id"]).subscribe(ingresoProducto => this.cargarFormulario(ingresoProducto),
          error => console.error(error));
      }
    });
  }

  get fi() { return this.formItem.value; }

  buscarProducto() {
    this.elegirProducto = true;
  }

  asignarProducto(event: IProducto) {
    this.elegirProducto = false;
    if (event) {
      this.formItem.patchValue({
        producto: event,
        unidad: event.items[0]
      });
    }
  }

  cargarFormulario(ingresoProducto: IIngresoProducto) {
    this.form.patchValue({
      idIngresoProducto: ingresoProducto.idIngresoProducto,
      fechaCreacion: new Date(ingresoProducto.fechaCreacion),
      comentario: ingresoProducto.comentario
    });
    this.items = ingresoProducto.items;
  }

  save() {
    let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    ingresoProducto.usuario = usuario;

    if (this.modoEdicion) {
      this.ingresoProductoService.updateIngresoProducto(ingresoProducto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      ingresoProducto.items = this.items;
      this.ingresoProductoService.createIngresoProducto(ingresoProducto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/ingresos-producto"]);
  }

  saveItem() {
    this.formItem.patchValue({
      unidad: this.fi.unidad.unidad,
      factor: this.fi.unidad.factor,
      producto: { items: [] }
    });

    let i: IItemIngresoProducto = Object.assign({}, this.formItem.value);

    if (this.modoEdicion) {

      let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);
      i.ingresoProducto = ingresoProducto;

      this.ingresoProductoService.createItem(i)
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

  deleteItem(i: IItemIngresoProducto) {
    if (this.modoEdicion) {
      this.ingresoProductoService.deleteItem(i.idItemIngresoProducto)
        .subscribe(data => this.onDeleteItemSuccess(i),
          error => console.log(error));
    } else {
      this.onDeleteItemSuccess(i);
    }
  }

  onDeleteItemSuccess(i: IItemIngresoProducto) {
    this.items.forEach((item, index) => {
      if (item.idItemIngresoProducto === i.idItemIngresoProducto) this.items.splice(index, 1);
    });
  }

}
