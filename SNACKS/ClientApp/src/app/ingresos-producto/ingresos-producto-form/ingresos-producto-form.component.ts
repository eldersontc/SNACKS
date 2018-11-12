import { Component, OnInit, Inject } from '@angular/core';
import { ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IngresosProductoService } from '../ingresos-producto.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemIngresoProducto, IIngresoProducto } from '../ingreso-producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { LotesService } from '../../lotes/lotes.service';
import { IItemLote } from '../../lotes/lote';
import { IAlmacen } from '../../almacenes/almacen';
import { AlmacenesService } from '../../almacenes/almacenes.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-ingresos-producto-form',
  templateUrl: './ingresos-producto-form.component.html',
  styleUrls: ['./ingresos-producto-form.component.css']
})
export class IngresosProductoFormComponent implements OnInit {

  modoEdicion: boolean;
  modoLectura: boolean;

  form: FormGroup;

  items: IItemIngresoProducto[] = [];
  almacenes: IAlmacen[] = [];
  login: ILogin;

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    notifierService: NotifierService,
    private ingresoProductoService: IngresosProductoService,
    private loteService: LotesService,
    private almacenService: AlmacenesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.notifier = notifierService;
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        this.getAlmacenes();
        return;
      } else {
        this.modoEdicion = true;
        if (params["mode"]) {
          this.modoLectura = true;
        }
        this.ingresoProductoService
          .getIngresoProducto(params["id"])
          .subscribe(ingresoProducto => this.cargarFormulario(ingresoProducto),
          error => console.error(error));
      }
    });
  }

  ngOnInit() {
    this.form = this.fb.group({
      idIngresoProducto: 0,
      fechaCreacion: new Date(),
      comentario: '',
      idLote: '',
      almacen: ''
    });
  }

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

  buscarLote() {
    let ingresoProducto: IIngresoProducto = Object.assign({}, this.form.value);

    this.loteService.getItems(ingresoProducto.idLote)
      .subscribe(data => this.cargarProductos(data),
        error => console.error(error));
  }

  cargarProductos(data: IItemLote[]) {
    this.items = [];
    data.forEach((d) => {
      this.items.push({
        producto: d.producto,
        cantidad: 0,
        unidad: d.producto.items[0].unidad,
        factor: d.producto.items[0].factor
      });
    });
  }

  setFactor(i: IItemIngresoProducto) {
    i.producto.items.forEach((ip) => {
      if (i.unidad.idUnidad == ip.unidad.idUnidad)
        i.factor = ip.factor
    });
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
    
    ingresoProducto.usuario = { idUsuario: this.login.id };
    ingresoProducto.items = this.items;

    if (this.modoEdicion) {
      this.ingresoProductoService.updateIngresoProducto(ingresoProducto)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    } else {
      this.ingresoProductoService.createIngresoProducto(ingresoProducto)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    }
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onSaveSuccess() {
    this.router.navigate(["/ingresos-producto"]);
  }

  deleteItem(i: IItemIngresoProducto) {
    this.items.forEach((item, index) => {
      if (item.producto.idProducto === i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }
}
