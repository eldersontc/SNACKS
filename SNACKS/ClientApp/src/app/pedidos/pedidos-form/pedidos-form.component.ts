import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PedidosService } from '../pedidos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPedido, IItemPedido } from '../pedido';
import { IFiltro, ILogin } from '../../generico/generico';
import { IPersona } from '../../personas/persona';
import { IProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';

@Component({
  selector: 'app-pedidos-form',
  templateUrl: './pedidos-form.component.html',
  styleUrls: ['./pedidos-form.component.css']
})
export class PedidosFormComponent implements OnInit {

  filtrosCliente: IFiltro[] = [];
  filtrosProducto: IFiltro[] = [];
  elegirCliente: boolean = false;
  elegirProducto: boolean = false;

  login: ILogin

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private pedidoService: PedidosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
  }

  modoEdicion: boolean = false;
  modoLectura: boolean = false;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemPedido[] = [];

  ngOnInit() {
    
    this.filtrosCliente.push({ k: 1, v: 'Cliente', n: 2 });
    this.filtrosProducto.push({ k: 2, v: 'Producto', b: false });

    if (this.login.tipo == 3) {
      this.filtrosCliente.push({ k: 6, v: this.login.nombrePersona, n: this.login.idPersona });
    }

    this.form = this.fb.group({
      idPedido: 0,
      fechaCreacion: new Date(),
      fechaPropuesta: new Date(),
      cliente: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      }),
      comentario: '',
      total: 0,
      estado: ''
    });
    if (this.login.tipo == 2) {
      this.form.patchValue({
        cliente: {
          idPersona: this.login.idPersona,
          razonSocial: this.login.nombrePersona
        }
      });
    }
    this.formItem = this.fb.group({
      producto: this.fb.group({
        idProducto: 0,
        nombre: '',
        items: []
      }),
      unidad: '',
      cantidad: '',
      factor: 0,
      total: 0
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        if (params["mode"]) {
          this.modoLectura = true;
        }
        this.pedidoService.getPedido(params["id"]).subscribe(pedido => this.cargarFormulario(pedido),
          error => console.error(error));
      }
    });
  }

  get fi() { return this.formItem.value; }

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

  asignarProducto(event: IProducto) {
    this.elegirProducto = false;
    if (event) {
      this.formItem.patchValue({
        producto: event,
        unidad: event.items[0]
      });
    }
  }

  cargarFormulario(pedido: IPedido) {
    this.form.patchValue({
      idPedido: pedido.idPedido,
      fechaCreacion: new Date(pedido.fechaCreacion),
      fechaPropuesta: new Date(pedido.fechaPropuesta),
      cliente: pedido.cliente,
      comentario: pedido.comentario,
      total: pedido.total,
      estado: pedido.estado
    });
    this.items = pedido.items;
  }

  save() {
    let pedido: IPedido = Object.assign({}, this.form.value);

    pedido.usuario = { idUsuario: this.login.id };
    pedido.items = this.items;

    if (this.modoEdicion) {
      this.pedidoService.updatePedido(pedido)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.pedidoService.createPedido(pedido)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/pedidos"]);
  }

  saveItem() {
    this.formItem.patchValue({
      unidad: this.fi.unidad.unidad,
      factor: this.fi.unidad.factor,
      producto: { items: [] }
    });

    let i: IItemPedido = Object.assign({}, this.formItem.value);

    this.onSaveItemSuccess(i);
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
    this.formItem.patchValue({ total: 0 });
  }

  deleteItem(i: IItemPedido) {
    this.onDeleteItemSuccess(i);
  }

  onDeleteItemSuccess(i: IItemPedido) {
    this.items.forEach((item, index) => {
      if (item.idItemPedido === i.idItemPedido) this.items.splice(index, 1);
    });
  }

}
