import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PedidosService } from '../pedidos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPedido, IItemPedido } from '../pedido';
import { Filtro } from '../../generico/generico';
import { IPersona } from '../../personas/persona';
import { IProducto, IItemProducto } from '../../productos/producto';
import { WebStorageService, LOCAL_STORAGE } from 'angular-webstorage-service';
import { IUsuario } from '../../usuarios/usuario';

@Component({
  selector: 'app-pedidos-form',
  templateUrl: './pedidos-form.component.html',
  styleUrls: ['./pedidos-form.component.css']
})
export class PedidosFormComponent implements OnInit {

  filtroCliente: Filtro = new Filtro(1, 'Cliente', 2);
  filtroProducto: Filtro = new Filtro(2, 'Producto', 0, new Date(), false);
  elegirCliente: boolean = false;
  elegirProducto: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private pedidoService: PedidosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean = false;
  modoLectura: boolean = false;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemPedido[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idPedido: 0,
      fechaCreacion: new Date(),
      fechaPropuesta: new Date(),
      cliente: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      }),
      comentario: '',
      total: 0
    });
    this.formItem = this.fb.group({
      producto: this.fb.group({
        idProducto: 0,
        nombre: '',
        items: []
      }),
      unidad: '',
      cantidad: '',
      factor: 0,
      total: ''
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

  asignarCliente(event: IPersona) {
    this.elegirCliente = false;
    if (event) {
      this.form.patchValue({
        cliente: event
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
      total: pedido.total
    });
    this.items = pedido.items;
  }

  save() {
    let pedido: IPedido = Object.assign({}, this.form.value);
    let usuario: IUsuario = Object.assign({}, { idUsuario: this.storage.get('login').id, nombre: '', clave: '', persona: null });

    pedido.usuario = usuario;
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

    //if (this.modoEdicion) {

    //  let pedido: IPedido = Object.assign({}, this.form.value);
    //  i.pedido = pedido;

    //  this.pedidoService.createItem(i)
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

  deleteItem(i: IItemPedido) {
    //if (this.modoEdicion) {
    //  this.pedidoService.deleteItem(i.idItemPedido)
    //    .subscribe(data => this.onDeleteItemSuccess(i),
    //      error => console.log(error));
    //} else {
      this.onDeleteItemSuccess(i);
    //}
  }

  onDeleteItemSuccess(i: IItemPedido) {
    this.items.forEach((item, index) => {
      if (item.idItemPedido === i.idItemPedido) this.items.splice(index, 1);
    });
  }

}
