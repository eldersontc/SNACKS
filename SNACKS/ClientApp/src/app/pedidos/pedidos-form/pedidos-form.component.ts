import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PedidosService } from '../pedidos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPedido, IItemPedido } from '../pedido';
import { Filtro } from '../../generico/generico';
import { IPersona } from '../../personas/persona';
import { IProducto, IItemProducto } from '../../productos/producto';

@Component({
  selector: 'app-pedidos-form',
  templateUrl: './pedidos-form.component.html',
  styleUrls: ['./pedidos-form.component.css']
})
export class PedidosFormComponent implements OnInit {

  filtroCliente: Filtro = new Filtro(1, 'Cliente', 2);
  elegirCliente: boolean = false;
  elegirProducto: boolean = false;

  constructor(private fb: FormBuilder,
    private pedidoService: PedidosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemPedido[] = [];

  ngOnInit() {
    this.form = this.fb.group({
      idPedido: 0,
      fechaCreacion: new Date(),
      cliente: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      })
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
      cliente: pedido.cliente
    });
    this.items = pedido.items;
  }

  save() {
    let pedido: IPedido = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.pedidoService.updatePedido(pedido)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      pedido.items = this.items;
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

    if (this.modoEdicion) {

      let pedido: IPedido = Object.assign({}, this.form.value);
      i.pedido = pedido;

      this.pedidoService.createItem(i)
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

  deleteItem(i: IItemPedido) {
    if (this.modoEdicion) {
      this.pedidoService.deleteItem(i.idItemPedido)
        .subscribe(data => this.onDeleteItemSuccess(i),
          error => console.log(error));
    } else {
      this.onDeleteItemSuccess(i);
    }
  }

  onDeleteItemSuccess(i: IItemPedido) {
    this.items.forEach((item, index) => {
      if (item.idItemPedido === i.idItemPedido) this.items.splice(index, 1);
    });
  }
}
