import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SalidasInsumoService } from '../salidas-insumo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemSalidaInsumo, ISalidaInsumo } from '../salida-insumo';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';
import { IAlmacen } from '../../almacenes/almacen';
import { AlmacenesService } from '../../almacenes/almacenes.service';
import { LotesService } from '../../lotes/lotes.service';
import { IItemLote } from '../../lotes/lote';

@Component({
  selector: 'app-salidas-insumo-form',
  templateUrl: './salidas-insumo-form.component.html',
  styleUrls: ['./salidas-insumo-form.component.css']
})
export class SalidasInsumoFormComponent implements OnInit {

  elegirProducto: boolean = false;
  modoEdicion: boolean;

  form: FormGroup;
  formItem: FormGroup;

  items: IItemSalidaInsumo[] = [];
  almacenes: IAlmacen[] = [];
  login: ILogin;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private salidaInsumoService: SalidasInsumoService,
    private almacenService: AlmacenesService,
    private loteService: LotesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        this.getAlmacenes();
        return;
      } else {
        this.modoEdicion = true;
        this.salidaInsumoService.getSalidaInsumo(params["id"]).subscribe(salidaInsumo => this.cargarFormulario(salidaInsumo),
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

  ngOnInit() {
    this.form = this.fb.group({
      idSalidaInsumo: 0,
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

  buscarLote() {
    let salidaInsumo: ISalidaInsumo = Object.assign({}, this.form.value);

    this.loteService.getItemsWithInsumos(salidaInsumo.idLote)
      .subscribe(data => this.cargarProductos(data),
        error => console.error(error));
  }

  cargarProductos(data: IItemLote[]) {
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

    });
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
          error => console.error(error));
    } else {
      this.salidaInsumoService.createSalidaInsumo(salidaInsumo)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/salidas-insumo"]);
  }

  saveItem() {
    this.formItem.patchValue({
      unidad: this.fi.unidad.unidad,
      factor: this.fi.unidad.factor,
      producto: { items: [] }
    });

    let i: IItemSalidaInsumo = Object.assign({}, this.formItem.value);

    //if (this.modoEdicion) {

    //  let salidaInsumo: ISalidaInsumo = Object.assign({}, this.form.value);
      //i.salidaInsumo = salidaInsumo;

    //  this.salidaInsumoService.createItem(i)
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

  deleteItem(i: IItemSalidaInsumo) {
    //if (this.modoEdicion) {
    //  this.salidaInsumoService.deleteItem(i.idItemSalidaInsumo)
    //    .subscribe(data => this.onDeleteItemSuccess(i),
    //      error => console.log(error));
    //} else {
      this.onDeleteItemSuccess(i);
    //}
  }

  onDeleteItemSuccess(i: IItemSalidaInsumo) {
    this.items.forEach((item, index) => {
      if (item.idItemSalidaInsumo === i.idItemSalidaInsumo) this.items.splice(index, 1);
    });
  }

}
