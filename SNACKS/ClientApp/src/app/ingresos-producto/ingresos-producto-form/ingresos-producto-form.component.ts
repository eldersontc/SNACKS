import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IngresosProductoService } from '../ingresos-producto.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemIngresoProducto, IIngresoProducto } from '../ingreso-producto';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';
import { LotesService } from '../../lotes/lotes.service';
import { IItemLote } from '../../lotes/lote';
import { IAlmacen } from '../../almacenes/almacen';
import { AlmacenesService } from '../../almacenes/almacenes.service';

@Component({
  selector: 'app-ingresos-producto-form',
  templateUrl: './ingresos-producto-form.component.html',
  styleUrls: ['./ingresos-producto-form.component.css']
})
export class IngresosProductoFormComponent implements OnInit {

  filtrosProducto: IFiltro[] = [];
  elegirProducto: boolean = false;

  login: ILogin;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private ingresoProductoService: IngresosProductoService,
    private loteService: LotesService,
    private almacenService: AlmacenesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        this.getAlmacenes();
        return;
      } else {
        this.modoEdicion = true;
        this.ingresoProductoService.getIngresoProducto(params["id"]).subscribe(ingresoProducto => this.cargarFormulario(ingresoProducto),
          error => console.error(error));
      }
    });
  }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemIngresoProducto[] = [];
  almacenes: IAlmacen[] = [];

  ngOnInit() {
    this.filtrosProducto.push({ k: 2, v: 'Producto', b: false });
    this.form = this.fb.group({
      idIngresoProducto: 0,
      fechaCreacion: new Date(),
      comentario: '',
      idLote: '',
      almacen: ''
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
  }

  get fi() { return this.formItem.value; }

  getAlmacenes() {
    this.almacenService.getAll()
      .subscribe(d => this.onGetAlmacenesSuccess(d), error => console.error(error));
  }

  onGetAlmacenesSuccess(d) {
    this.almacenes = d;
    if (this.almacenes.length > 0) {
      this.form.patchValue({ almacen: this.almacenes[0] });
    }
  }

  buscarProducto() {
    this.elegirProducto = true;
  }

  buscarLote() {
    let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);

    this.loteService.getItems(ingresoProducto.idLote)
      .subscribe(data => this.cargarProductos(data),
        error => console.error(error));
  }

  cargarProductos(data: IItemLote[]) {
    data.forEach((d) => {
      this.items.push({
        producto: d.producto,
        cantidad: 0,
        unidad: d.producto.items[0].unidad
      });
    });
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
      comentario: ingresoProducto.comentario,
      almacen: ingresoProducto.almacen,
      idLote: ingresoProducto.idLote
    });
    this.almacenes.push(ingresoProducto.almacen);
    this.items = ingresoProducto.items;
    this.items.forEach((i) => {
      i.producto.items.push({ unidad: i.unidad });
    });
  }

  save() {
    let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    ingresoProducto.usuario = usuario;
    ingresoProducto.items = this.items;

    if (this.modoEdicion) {
      this.ingresoProductoService.updateIngresoProducto(ingresoProducto)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
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

    //if (this.modoEdicion) {

    //  let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);
    //  i.ingresoProducto = ingresoProducto;

    //  this.ingresoProductoService.createItem(i)
    //    .subscribe(data => this.onSaveItemSuccess(i),
    //      error => console.error(error));

    //} else {
      this.onSaveItemSuccess(i);
    //}
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemIngresoProducto) {
    //if (this.modoEdicion) {
    //  this.ingresoProductoService.deleteItem(i.idItemIngresoProducto)
    //    .subscribe(data => this.onDeleteItemSuccess(i),
    //      error => console.log(error));
    //} else {
      this.onDeleteItemSuccess(i);
    //}
  }

  onDeleteItemSuccess(i: IItemIngresoProducto) {
    this.items.forEach((item, index) => {
      if (item.idItemIngresoProducto === i.idItemIngresoProducto) this.items.splice(index, 1);
    });
  }

}
