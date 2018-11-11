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

@Component({
  selector: 'app-ingresos-insumo-form',
  templateUrl: './ingresos-insumo-form.component.html',
  styleUrls: ['./ingresos-insumo-form.component.css']
})
export class IngresosInsumoFormComponent implements OnInit {

  elegirProducto: boolean = false;
  modoEdicion: boolean;

  form: FormGroup;
  formItem: FormGroup;

  items: IItemIngresoInsumo[] = [];
  almacenes: IAlmacen[] = [];
  cajas: ICaja[] = []
  login: ILogin;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private ingresoInsumoService: IngresosInsumoService,
    private almacenService: AlmacenesService,
    private cajaService: CajasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        this.getAlmacenes();
        this.getCajas();
        return;
      } else {
        this.modoEdicion = true;
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
    this.formItem = this.fb.group({
      producto: this.fb.group({
        idProducto: 0,
        nombre: '',
        items: []
      }),
      unidad: '',
      cantidad: '',
      costo: '',
      factor: 0
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
  }

  save() {
    let ingresoInsumo: IIngresoInsumo = Object.assign({}, this.form.value);
    
    ingresoInsumo.usuario = { idUsuario: this.login.id };
    ingresoInsumo.items = this.items;

    if (this.modoEdicion) {
      this.ingresoInsumoService.updateIngresoInsumo(ingresoInsumo)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.ingresoInsumoService.createIngresoInsumo(ingresoInsumo)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/ingresos-insumo"]);
  }

  saveItem() {
    this.formItem.patchValue({
      unidad: this.fi.unidad.unidad,
      factor: this.fi.unidad.factor,
      producto: { items: [] }
    });

    let i: IItemIngresoInsumo = Object.assign({}, this.formItem.value);

    this.onSaveItemSuccess(i);
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemIngresoInsumo) {
    this.onDeleteItemSuccess(i);
  }

  onDeleteItemSuccess(i: IItemIngresoInsumo) {
    this.items.forEach((item, index) => {
      if (item.idItemIngresoInsumo === i.idItemIngresoInsumo) this.items.splice(index, 1);
    });
  }

}
