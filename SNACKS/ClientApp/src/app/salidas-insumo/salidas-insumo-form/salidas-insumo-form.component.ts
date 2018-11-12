import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SalidasInsumoService } from '../salidas-insumo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemSalidaInsumo, ISalidaInsumo } from '../salida-insumo';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IAlmacen } from '../../almacenes/almacen';
import { AlmacenesService } from '../../almacenes/almacenes.service';
import { LotesService } from '../../lotes/lotes.service';
import { IItemLote } from '../../lotes/lote';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-salidas-insumo-form',
  templateUrl: './salidas-insumo-form.component.html',
  styleUrls: ['./salidas-insumo-form.component.css']
})
export class SalidasInsumoFormComponent implements OnInit {

  elegirProducto: boolean = false;
  modoEdicion: boolean;
  modoLectura: boolean;

  form: FormGroup;
  formItem: FormGroup;

  items: IItemSalidaInsumo[] = [];
  almacenes: IAlmacen[] = [];
  login: ILogin;
  productos: IProducto[] = [];
  producto: IProducto;
  insumo: IProducto = {};

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    notifierService: NotifierService,
    private salidaInsumoService: SalidasInsumoService,
    private almacenService: AlmacenesService,
    private loteService: LotesService,
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
        this.salidaInsumoService.getSalidaInsumo(params["id"])
          .subscribe(salidaInsumo => this.cargarFormulario(salidaInsumo),
          error => console.error(error));
      }
    });
  }

  getAlmacenes() {
    this.almacenService.getAll()
      .subscribe(d => this.onGetAlmacenesSuccess(d),
        error => console.error(error));
  }

  onGetAlmacenesSuccess(d) {
    this.almacenes = d;
    if (this.almacenes.length > 0) {
      this.form.patchValue({ almacen: this.almacenes[0] });
    }
  }

  ngOnInit() {
    this.form = this.fb.group({
      idSalidaInsumo: 0,
      fechaCreacion: new Date(),
      comentario: '',
      idLote: '',
      almacen: ''
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

  buscarLote() {
    let salidaInsumo: ISalidaInsumo = Object.assign({}, this.form.value);

    this.loteService.getItemsWithInsumos(salidaInsumo.idLote)
      .subscribe(data => this.cargarProductos(data),
        error => console.error(error));
  }

  cargarProductos(data: IItemLote[]) {
    this.items = [];
    data.forEach((d) => {
      d.producto.insumos.forEach((i) => {
        this.items.push({
          producto: d.producto,
          insumo: i.insumo,
          cantidad: 0,
          unidad: i.insumo.items[0].unidad,
          factor: i.insumo.items[0].factor
        });
      });
      this.productos.push({
        idProducto: d.producto.idProducto,
        nombre: d.producto.nombre
      });
    });
    this.producto = this.productos[0];
  }

  setFactor(i: IItemSalidaInsumo) {
    i.insumo.items.forEach((ip) => {
      if (i.unidad.idUnidad == ip.unidad.idUnidad)
        i.factor = ip.factor
    });
  }

  cargarFormulario(salidaInsumo: ISalidaInsumo) {
    this.form.patchValue({
      idSalidaInsumo: salidaInsumo.idSalidaInsumo,
      fechaCreacion: new Date(salidaInsumo.fechaCreacion),
      comentario: salidaInsumo.comentario,
      almacen: salidaInsumo.almacen,
      idLote: salidaInsumo.idLote
    });
    this.items = salidaInsumo.items;
    this.almacenes.push(salidaInsumo.almacen);
    this.items.forEach((i) => {
      i.insumo.items.push({ unidad: i.unidad });
    });
  }

  save() {
    let salidaInsumo: ISalidaInsumo = Object.assign({}, this.form.value);
    
    salidaInsumo.usuario = { idUsuario: this.login.id };
    salidaInsumo.items = this.items;

    if (this.modoEdicion) {
      this.salidaInsumoService.updateSalidaInsumo(salidaInsumo)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    } else {
      this.salidaInsumoService.createSalidaInsumo(salidaInsumo)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    }
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onSaveSuccess() {
    this.router.navigate(["/salidas-insumo"]);
  }

  saveItem() {
    this.items.push({
      producto: this.producto,
      insumo: this.insumo,
      unidad: this.insumo.items[0].unidad,
      factor: this.insumo.items[0].factor,
      cantidad: 0
    });
    this.insumo = {};
  }

  deleteItem(i: IItemSalidaInsumo) {
    this.items.forEach((item, index) => {
      if (item.insumo.idProducto === i.insumo.idProducto
        && item.producto.idProducto == i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }

}
