import { Component, OnInit, Inject } from '@angular/core';
import { Filtro } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SalidasProductoService } from '../salidas-producto.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemSalidaProducto, ISalidaProducto } from '../salida-producto';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';

@Component({
  selector: 'app-salidas-producto-form',
  templateUrl: './salidas-producto-form.component.html',
  styleUrls: ['./salidas-producto-form.component.css']
})
export class SalidasProductoFormComponent implements OnInit {

  filtroProducto: Filtro = new Filtro(2, 'Producto', 0, new Date(), false);
  elegirProducto: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private salidaProductoService: SalidasProductoService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemSalidaProducto[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idSalidaProducto: 0,
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
        this.salidaProductoService.getSalidaProducto(params["id"]).subscribe(salidaProducto => this.cargarFormulario(salidaProducto),
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

  cargarFormulario(salidaProducto: ISalidaProducto) {
    this.form.patchValue({
      idSalidaProducto: salidaProducto.idSalidaProducto,
      fechaCreacion: new Date(salidaProducto.fechaCreacion),
      comentario: salidaProducto.comentario
    });
    this.items = salidaProducto.items;
  }

  save() {
    let salidaProducto: ISalidaProducto = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    salidaProducto.usuario = usuario;

    if (this.modoEdicion) {
      this.salidaProductoService.updateSalidaProducto(salidaProducto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      salidaProducto.items = this.items;
      this.salidaProductoService.createSalidaProducto(salidaProducto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/salidas-producto"]);
  }

  saveItem() {
    this.formItem.patchValue({
      unidad: this.fi.unidad.unidad,
      factor: this.fi.unidad.factor,
      producto: { items: [] }
    });

    let i: IItemSalidaProducto = Object.assign({}, this.formItem.value);

    if (this.modoEdicion) {

      let salidaProducto: ISalidaProducto = Object.assign({}, this.form.value);
      i.salidaProducto = salidaProducto;

      this.salidaProductoService.createItem(i)
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

  deleteItem(i: IItemSalidaProducto) {
    if (this.modoEdicion) {
      this.salidaProductoService.deleteItem(i.idItemSalidaProducto)
        .subscribe(data => this.onDeleteItemSuccess(i),
          error => console.log(error));
    } else {
      this.onDeleteItemSuccess(i);
    }
  }

  onDeleteItemSuccess(i: IItemSalidaProducto) {
    this.items.forEach((item, index) => {
      if (item.idItemSalidaProducto === i.idItemSalidaProducto) this.items.splice(index, 1);
    });
  }

}
