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

  filtrosProducto: IFiltro[] = [];
  elegirProducto: boolean = false;

  login: ILogin;

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

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemLote[] = [];

  ngOnInit() {
    this.filtrosProducto.push({ k: 2, v: 'Producto', b: false });
    this.form = this.fb.group({
      idLote: 0,
      fecha: new Date()
    });
    this.formItem = this.fb.group({
      producto: this.fb.group({
        idProducto: 0,
        nombre: '',
        items: []
      })
    });
  }

  get fi() { return this.formItem.value; }

  buscarProducto() {
    this.elegirProducto = true;
  }

  asignarProducto(e: IProducto) {
    this.elegirProducto = false;
    if (e) {
      this.formItem.patchValue({
        producto: e
      });
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
    let i: IItemLote = Object.assign({}, this.formItem.value);

    this.onSaveItemSuccess(i);
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemLote) {
    this.onDeleteItemSuccess(i);
  }

  onDeleteItemSuccess(i: IItemLote) {
    this.items.forEach((item, index) => {
      if (item.idItemLote === i.idItemLote) this.items.splice(index, 1);
    });
  }

}
