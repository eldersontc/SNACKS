import { Component, OnInit, Inject } from '@angular/core';
import { IFiltro, ILogin } from '../../generico/generico';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LotesService } from '../lotes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemLote, ILote } from '../lote';
import { IProducto } from '../../productos/producto';

@Component({
  selector: 'app-lotes-form',
  templateUrl: './lotes-form.component.html',
  styleUrls: ['./lotes-form.component.css']
})
export class LotesFormComponent implements OnInit {

  modoEdicion: boolean = false;
  elegirProducto: boolean = false;

  form: FormGroup;
  login: ILogin;
  items: IItemLote[] = [];
  producto: IProducto = {};

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private loteService: LotesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.loteService.getLote(params["id"]).subscribe(lote => this.cargarFormulario(lote),
          error => console.error(error));
      }
    });
  }

  ngOnInit() {
    this.form = this.fb.group({
      idLote: 0,
      fecha: new Date()
    });
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

  cargarFormulario(lote: ILote) {
    this.form.patchValue({
      idLote: lote.idLote,
      fechaCreacion: new Date(lote.fecha)
    });
    this.items = lote.items;
  }

  save() {
    let lote: ILote = Object.assign({}, this.form.value);

    lote.usuario = { idUsuario: this.login.id };
    lote.items = this.items;

    if (this.modoEdicion) {
      this.loteService.updateLote(lote)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.loteService.createLote(lote)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/lotes"]);
  }

  saveItem() {
    this.items.push({ producto: this.producto });
    this.producto = {};
  }

  deleteItem(i: IItemLote) {
    this.items.forEach((item, index) => {
      if (item.producto.idProducto === i.producto.idProducto)
        this.items.splice(index, 1);
    });
  }

}
