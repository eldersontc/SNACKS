import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { ICaja } from './caja';
import { CajasService } from './cajas.service';

@Component({
  selector: 'app-cajas',
  templateUrl: './cajas.component.html',
  styleUrls: ['./cajas.component.css']
})
export class CajasComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  cajas: ICaja[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: ICaja;

  columnas: string[][] = [
    ['L', 'Nombre'],
    ['L', 'Saldo']];
  atributos: string[][] = [
    ['S', 'L', 'nombre'],
    ['I', 'L', 'saldo']]

  constructor(private cajaService: CajasService) { }

  ngOnInit() {
    this.getCajas();
  }

  getCajas() {
    this.seleccion = undefined;
    this.cajaService.getCajas({
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

  onGetSuccess(data: IListaRetorno<ICaja>) {
    this.cajas = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getCajas();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getCajas();
    }
  }

  deleteCaja() {
    this.cajaService.deleteCaja(this.seleccion.idCaja).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getCajas();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }

}
