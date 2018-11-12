import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PedidosService } from '../pedidos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPedido, IItemPedido } from '../pedido';
import { IFiltro, ILogin } from '../../generico/generico';
import { IPersona } from '../../personas/persona';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { NotifierService } from 'angular-notifier';

@Component({
  selector: 'app-pedidos-form',
  templateUrl: './pedidos-form.component.html',
  styleUrls: ['./pedidos-form.component.css']
})
export class PedidosFormComponent implements OnInit {

  fCliente: IFiltro[] = [];
  elegirCliente: boolean = false;
  elegirProducto: boolean = false;

  login: ILogin

  private readonly notifier: NotifierService;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    notifierService: NotifierService,
    private pedidoService: PedidosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.notifier = notifierService;
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        if (params["mode"]) {
          this.modoLectura = true;
        }
        this.pedidoService.getPedido(params["id"])
          .subscribe(pedido => this.cargarFormulario(pedido),
          error => console.error(error));
      }
    });
  }

  modoEdicion: boolean = false;
  modoLectura: boolean = false;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemPedido[] = [];

  producto: IProducto = { };

  ngOnInit() {
    
    this.fCliente.push({
        k: 1,
        v: 'Cliente',
        n: 2
      });
    
    if (this.login.tipo == 3) {
      this.fCliente.push({
        k: 6,
        v: this.login.nombrePersona,
        n: this.login.idPersona
      });
    }

    this.form = this.fb.group({
      idPedido: 0,
      fechaPropuesta: new Date(),
      cliente: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      }),
      comentario: '',
      total: 0
    });

    if (this.login.tipo == 2) {
      this.form.patchValue({
        cliente: {
          idPersona: this.login.idPersona,
          razonSocial: this.login.nombrePersona
        }
      });
    }

  }

  buscarCliente() {
    this.elegirCliente = true;
  }

  buscarProducto() {
    this.elegirProducto = true;
  }

  asignarCliente(e: IPersona) {
    this.elegirCliente = false;
    if (e) {
      this.form.patchValue({
        cliente: e
      });
    }
  }

  asignarProducto(e: IProducto) {
    this.elegirProducto = false;
    if (e) {
      this.producto = e;
    }
  }

  cargarFormulario(pedido: IPedido) {
    this.form.patchValue({
      idPedido: pedido.idPedido,
      fechaPropuesta: new Date(pedido.fechaPropuesta),
      cliente: pedido.cliente,
      comentario: pedido.comentario,
      total: pedido.total
    });
    this.items = pedido.items;
    this.items.forEach((i) => {
      i.producto.items.push({ unidad: i.unidad });
    });
  }

  save() {
    let pedido: IPedido = Object.assign({}, this.form.value);

    pedido.usuario = { idUsuario: this.login.id };
    pedido.items = this.items;

    if (this.modoEdicion) {
      this.pedidoService.updatePedido(pedido)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    } else {
      this.pedidoService.createPedido(pedido)
        .subscribe(data => this.onSaveSuccess(),
        error => this.showError(error));
    }
  }

  showError(error) {
    this.notifier.notify('error', error.error);
  }

  onSaveSuccess() {
    this.router.navigate(["/pedidos"]);
  }

  setFactor(i: IItemPedido) {
    i.producto.items.forEach((ip) => {
      if (i.unidad.idUnidad == ip.unidad.idUnidad)
        i.factor = ip.factor
    });
  }

  saveItem() {
    this.items.push({
      producto: this.producto,
      unidad: this.producto.items[0].unidad,
      factor: this.producto.items[0].factor,
      cantidad: 0,
      total: 0
    });
    this.producto = {};
  }

  deleteItem(i: IItemPedido) {
    this.items.forEach((item, index) => {
      if (item.producto.idProducto === i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }

}
