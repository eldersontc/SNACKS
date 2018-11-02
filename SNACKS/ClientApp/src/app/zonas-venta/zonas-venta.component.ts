import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IZonaVenta } from './zonaVenta';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { ZonasVentaService } from './zonas-venta.service';

@Component({
  selector: 'app-zonas-venta',
  templateUrl: './zonas-venta.component.html',
  styleUrls: ['./zonas-venta.component.css']
})
export class ZonasVentaComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  zonasVenta: IZonaVenta[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IZonaVenta;

  columnas: string[][] = [
    ['L', 'Nombre']];
  atributos: string[][] = [
    ['S', 'L', 'nombre']]

  constructor(private zonaVentaService: ZonasVentaService) { }

  ngOnInit() {
    this.getZonasVenta();
  }

  getZonasVenta() {
    this.seleccion = undefined;
    this.zonaVentaService.getZonasVenta({
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

  onGetSuccess(data: IListaRetorno<IZonaVenta>) {
    this.zonasVenta = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getZonasVenta();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.filtros.push({ k: this.criterio, v: this.busqueda });
      this.busqueda = '';
      this.getZonasVenta();
    }
  }

  deleteZonaVenta() {
    this.zonaVentaService.deleteZonaVenta(this.seleccion.idZonaVenta).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getZonasVenta();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }

}
