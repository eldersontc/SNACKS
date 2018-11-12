import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SalidasProductoService } from '../salidas-producto.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemSalidaProducto, ISalidaProducto } from '../salida-producto';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';
import { IAlmacen } from '../../almacenes/almacen';
import { AlmacenesService } from '../../almacenes/almacenes.service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-salidas-producto-form',
  templateUrl: './salidas-producto-form.component.html',
  styleUrls: ['./salidas-producto-form.component.css']
})
export class SalidasProductoFormComponent implements OnInit {
  
  elegirProducto: boolean = false;
  modoEdicion: boolean = false;
  modoLectura: boolean = false;

  form: FormGroup;
  items: IItemSalidaProducto[] = [];
  almacenes: IAlmacen[] = [];
  producto: IProducto = {};
  login: ILogin;

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    notifierService: NotifierService,
    private salidaProductoService: SalidasProductoService,
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
        this.salidaProductoService.getSalidaProducto(params["id"])
          .subscribe(salidaProducto => this.cargarFormulario(salidaProducto),
          error => console.error(error));
      }
    });
  }

  ngOnInit() {
    this.form = this.fb.group({
      idSalidaProducto: 0,
      fechaCreacion: new Date(),
      comentario: '',
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

  buscarProducto() {
    this.elegirProducto = true;
  }

  asignarProducto(e: IProducto) {
    this.elegirProducto = false;
    if (e) {
      this.producto = e;
    }
  }

  cargarFormulario(salidaProducto: ISalidaProducto) {
    this.form.patchValue({
      idSalidaProducto: salidaProducto.idSalidaProducto,
      fechaCreacion: new Date(salidaProducto.fechaCreacion),
      comentario: salidaProducto.comentario,
      almacen: salidaProducto.almacen
    });
    this.items = salidaProducto.items;
    this.almacenes.push(salidaProducto.almacen);
    this.items.forEach((i) => {
      i.producto.items.push({ unidad: i.unidad });
    });
  }

  save() {
    let salidaProducto: ISalidaProducto = Object.assign({}, this.form.value);
    
    salidaProducto.usuario = { idUsuario: this.login.id };
    salidaProducto.items = this.items;

    if (this.modoEdicion) {
      this.salidaProductoService.updateSalidaProducto(salidaProducto)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    } else {
      this.salidaProductoService.createSalidaProducto(salidaProducto)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    }
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onSaveSuccess() {
    this.router.navigate(["/salidas-producto"]);
  }

  saveItem() {
    this.items.push({
      producto: this.producto,
      unidad: this.producto.items[0].unidad,
      factor: this.producto.items[0].factor,
      cantidad: 0
    });
    this.producto = {};
  }

  setFactor(i: IItemSalidaProducto) {
    i.producto.items.forEach((ip) => {
      if (i.unidad.idUnidad == ip.unidad.idUnidad)
        i.factor = ip.factor
    });
  }

  deleteItem(i: IItemSalidaProducto) {
    this.items.forEach((item, index) => {
      if (item.producto.idProducto === i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }

}
