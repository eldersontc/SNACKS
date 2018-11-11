import { Component, OnInit } from '@angular/core';
import { IMovimientoCaja } from './movimiento-caja';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { MovimientosCajaService } from './movimientos-caja.service';
import { ActivatedRoute } from '@angular/router';
import { CajasService } from '../cajas/cajas.service';

@Component({
  selector: 'app-movimientos-caja',
  templateUrl: './movimientos-caja.component.html',
  styleUrls: ['./movimientos-caja.component.css']
})
export class MovimientosCajaComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  movimientosCaja: IMovimientoCaja[];
  extern: IFiltro[] = [];
  filtros: IFiltro[] = [];
  criterio: number = 2;
  busqueda: Date;
  seleccion: IMovimientoCaja;


  columnas: string[][] = [
    ['L', 'Nro.'],
    ['L', 'Fecha.'],
    ['L', 'Glosa'],
    ['L', 'Importe']];
  atributos: string[][] = [
    ['I', 'L', 'idMovimientoCaja'],
    ['D', 'L', 'fecha'],
    ['S', 'L', 'glosa'],
    ['I', 'L', 'importe']]

  constructor(private movimientoCajaService: MovimientosCajaService,
    private cajaService: CajasService,
    private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.cajaService.getCaja(params["id"])
          .subscribe(caja => this.getCajaSuccess(caja),
          error => console.error(error));
      }
    });
  }

  ngOnInit() { }

  getCajaSuccess(caja) {
    this.extern.push({ k: 1, v: caja.nombre, n: caja.idCaja });
    this.getMovimientosCaja();
  }

  seleccionFecha() {
    this.filtros.push({ k: this.criterio, d: this.busqueda });
    this.getMovimientosCaja();
  }

  getMovimientosCaja() {
    this.seleccion = undefined;
    this.movimientoCajaService.getMovimientosCaja({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros.concat(this.extern)
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IMovimientoCaja>) {
    this.movimientosCaja = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getMovimientosCaja();
  }

}
