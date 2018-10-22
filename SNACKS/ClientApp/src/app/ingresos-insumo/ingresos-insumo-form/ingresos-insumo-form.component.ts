import { Component, OnInit, Inject } from '@angular/core';
import { Filtro } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { IngresosInsumoService } from '../ingresos-insumo.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemIngresoInsumo, IIngresoInsumo } from '../ingreso-insumo';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';

@Component({
  selector: 'app-ingresos-insumo-form',
  templateUrl: './ingresos-insumo-form.component.html',
  styleUrls: ['./ingresos-insumo-form.component.css']
})
export class IngresosInsumoFormComponent implements OnInit {

  filtroProducto: Filtro = new Filtro(2, 'Insumo', 0, new Date(), true);
  elegirProducto: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private ingresoInsumoService: IngresosInsumoService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemIngresoInsumo[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idIngresoInsumo: 0,
      fechaCreacion: new Date(),
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
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.ingresoInsumoService.getIngresoInsumo(params["id"]).subscribe(ingresoInsumo => this.cargarFormulario(ingresoInsumo),
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

  cargarFormulario(ingresoInsumo: IIngresoInsumo) {
    this.form.patchValue({
      idIngresoInsumo: ingresoInsumo.idIngresoInsumo,
      fechaCreacion: new Date(ingresoInsumo.fechaCreacion),
      comentario: ingresoInsumo.comentario,
      costo: ingresoInsumo.costo
    });
    this.items = ingresoInsumo.items;
  }

  save() {
    let ingresoInsumo: IIngresoInsumo = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    ingresoInsumo.usuario = usuario;

    if (this.modoEdicion) {
      this.ingresoInsumoService.updateIngresoInsumo(ingresoInsumo)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      ingresoInsumo.items = this.items;
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

    if (this.modoEdicion) {

      let ingresoInsumo: IIngresoInsumo = Object.assign({}, this.form.value);
      i.ingresoInsumo = ingresoInsumo;

      this.ingresoInsumoService.createItem(i)
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

  deleteItem(i: IItemIngresoInsumo) {
    if (this.modoEdicion) {
      this.ingresoInsumoService.deleteItem(i.idItemIngresoInsumo)
        .subscribe(data => this.onDeleteItemSuccess(i),
          error => console.log(error));
    } else {
      this.onDeleteItemSuccess(i);
    }
  }

  onDeleteItemSuccess(i: IItemIngresoInsumo) {
    this.items.forEach((item, index) => {
      if (item.idItemIngresoInsumo === i.idItemIngresoInsumo) this.items.splice(index, 1);
    });
  }

}
