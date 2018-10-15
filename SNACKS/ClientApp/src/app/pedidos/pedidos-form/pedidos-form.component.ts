import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PedidosService } from '../pedidos.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPedido } from '../pedido';
import { Filtro } from '../../generico/generico';
import { IPersona } from '../../personas/persona';

@Component({
  selector: 'app-pedidos-form',
  templateUrl: './pedidos-form.component.html',
  styleUrls: ['./pedidos-form.component.css']
})
export class PedidosFormComponent implements OnInit {

  filtroCliente: Filtro = new Filtro(1, 'Cliente', 2);
  elegirCliente: boolean = false;

  constructor(private fb: FormBuilder,
    private pedidoService: PedidosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  formGroup: FormGroup;

  ngOnInit() {
    this.formGroup = this.fb.group({
      idPedido: 0,
      fechaCreacion: new Date(),
      cliente: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      })
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

  get f() { return this.formGroup.controls; }

  buscarCliente() {
    this.elegirCliente = true;
  }

  asignarCliente(event: IPersona) {
    this.elegirCliente = false;
    if (event) {
      this.formGroup.patchValue({
        cliente: event
      });
    }
  }

  cargarFormulario(pedido: IPedido) {
    this.formGroup.patchValue({
      idPedido: pedido.idPedido,
      fechaCreacion: new Date(pedido.fechaCreacion),
      cliente: pedido.cliente
    });
  }

  save() {
    let pedido: IPedido = Object.assign({}, this.formGroup.value);

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

}
