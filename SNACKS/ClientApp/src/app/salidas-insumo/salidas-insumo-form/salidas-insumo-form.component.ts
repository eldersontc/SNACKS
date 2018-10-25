import { Component, OnInit, Inject } from '@angular/core';
import { Filtro } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SalidasInsumoService } from '../salidas-insumo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemSalidaInsumo, ISalidaInsumo } from '../salida-insumo';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';

@Component({
  selector: 'app-salidas-insumo-form',
  templateUrl: './salidas-insumo-form.component.html',
  styleUrls: ['./salidas-insumo-form.component.css']
})
export class SalidasInsumoFormComponent implements OnInit {

  filtroProducto: Filtro = new Filtro(2, 'Insumo', 0, new Date(), true);
  elegirProducto: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private salidaInsumoService: SalidasInsumoService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemSalidaInsumo[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idSalidaInsumo: 0,
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
        this.salidaInsumoService.getSalidaInsumo(params["id"]).subscribe(salidaInsumo => this.cargarFormulario(salidaInsumo),
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

  cargarFormulario(salidaInsumo: ISalidaInsumo) {
    this.form.patchValue({
      idSalidaInsumo: salidaInsumo.idSalidaInsumo,
      fechaCreacion: new Date(salidaInsumo.fechaCreacion),
      comentario: salidaInsumo.comentario
    });
    this.items = salidaInsumo.items;
  }

  save() {
    let salidaInsumo: ISalidaInsumo = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    salidaInsumo.usuario = usuario;
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
