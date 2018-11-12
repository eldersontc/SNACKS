import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { IFiltro, ILogin, IListaRetorno } from '../generico/generico';
import { ILote } from './lote';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { LotesService } from './lotes.service';

@Component({
  selector: 'app-lotes',
  templateUrl: './lotes.component.html',
  styleUrls: ['./lotes.component.css']
})
export class LotesComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  lotes: ILote[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: Date;
  seleccion: ILote;
  login: ILogin;

  columnas: string[][] = [
    ['L', 'N° Lote'],
    ['L', 'Creado Por'],
    ['L', 'Fecha']];
  atributos: string[][] = [
    ['I', 'L', 'idLote'],
    ['S', 'L', 'usuario', 'nombre'],
    ['D', 'L', 'fecha']]

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private loteService: LotesService) {
    this.login = this.storage.get('login');
  }

  ngOnInit() {
    this.getLotes();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getLotes();
  }

  getLotes() {
    this.seleccion = undefined;
    this.loteService.getLotes({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros.concat(this.extern || [])
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  seleccionar(e) {
    this.seleccion = e;
  }

  onGetSuccess(data: IListaRetorno<ILote>) {
    this.lotes = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getLotes();
  }

  deleteLote() {
    this.loteService.deleteLote(this.seleccion.idLote).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getLotes();
  }

}
