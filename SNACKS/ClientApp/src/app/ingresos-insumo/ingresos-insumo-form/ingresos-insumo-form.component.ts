import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IngresosInsumoService } from '../ingresos-insumo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemIngresoInsumo, IIngresoInsumo } from '../ingreso-insumo';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { AlmacenesService } from '../../almacenes/almacenes.service';
import { CajasService } from '../../cajas/cajas.service';
import { IAlmacen } from '../../almacenes/almacen';
import { ICaja } from '../../cajas/caja';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-ingresos-insumo-form',
  templateUrl: './ingresos-insumo-form.component.html',
  styleUrls: ['./ingresos-insumo-form.component.css']
})
export class IngresosInsumoFormComponent implements OnInit {

  elegirProducto: boolean = false;
  modoEdicion: boolean = false;
  modoLectura: boolean = false;

  form: FormGroup;
  items: IItemIngresoInsumo[] = [];
  almacenes: IAlmacen[] = [];
  cajas: ICaja[] = [];
  login: ILogin;
  insumo: IProducto = {};

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    notifierService: NotifierService,
    private fb: FormBuilder,
    private ingresoInsumoService: IngresosInsumoService,
    private almacenService: AlmacenesService,
    private cajaService: CajasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.notifier = notifierService;
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        this.getAlmacenes();
        this.getCajas();
        return;
      } else {
        this.modoEdicion = true;
        if (params["mode"]) {
          this.modoLectura = true;
        }
        this.ingresoInsumoService.getIngresoInsumo(params["id"])
          .subscribe(ingresoInsumo => this.cargarFormulario(ingresoInsumo),
          error => console.error(error));
      }
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

  getCajas() {
    this.cajaService.getAll()
      .subscribe(d => this.onGetCajasSuccess(d), error => console.error(error));
  }

  onGetCajasSuccess(d) {
    this.cajas = d;
    if (this.cajas.length > 0) {
      this.form.patchValue({ caja: this.cajas[0] });
    }
  }

  ngOnInit() {
    this.form = this.fb.group({
      idIngresoInsumo: 0,
      fechaCreacion: new Date(),
      almacen: '',
      caja: '',
      comentario: '',
      costo: 0
    });
  }

  buscarProducto() {
    this.elegirProducto = true;
  }

  asignarProducto(e: IProducto) {
    this.elegirProducto = false;
    if (e) {
      this.insumo = e;
    }
  }

  cargarFormulario(ingresoInsumo: IIngresoInsumo) {
    this.form.patchValue({
      idIngresoInsumo: ingresoInsumo.idIngresoInsumo,
      fechaCreacion: new Date(ingresoInsumo.fechaCreacion),
      comentario: ingresoInsumo.comentario,
      costo: ingresoInsumo.costo,
      almacen: ingresoInsumo.almacen,
      caja: ingresoInsumo.caja
    });
    this.items = ingresoInsumo.items;
    this.almacenes.push(ingresoInsumo.almacen);
    this.cajas.push(ingresoInsumo.caja);
    this.items.forEach((i) => {
      i.producto.items.push({ unidad: i.unidad })
    });
  }

  save() {
    let ingresoInsumo: IIngresoInsumo = Object.assign({}, this.form.value);
    
    ingresoInsumo.usuario = { idUsuario: this.login.id };
    ingresoInsumo.items = this.items;

    if (this.modoEdicion) {
      this.ingresoInsumoService.updateIngresoInsumo(ingresoInsumo)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    } else {
      this.ingresoInsumoService.createIngresoInsumo(ingresoInsumo)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    }
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onSaveSuccess() {
    this.router.navigate(["/ingresos-insumo"]);
  }

  saveItem() {
    this.items.push({
      producto: this.insumo,
      unidad: this.insumo.items[0].unidad,
      factor: this.insumo.items[0].factor,
      cantidad: 0,
      costo: 0
    });
    this.insumo = {};
  }

  setFactor(i: IItemIngresoInsumo) {
    i.producto.items.forEach((ip) => {
      if (i.unidad.idUnidad == ip.unidad.idUnidad)
        i.factor = ip.factor
    });
  }

  deleteItem(i: IItemIngresoInsumo) {
    this.items.forEach((item, index) => {
      if (item.producto.idProducto === i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }

}
