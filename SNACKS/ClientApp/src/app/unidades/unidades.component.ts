import { Component, OnInit } from '@angular/core';
import { IUnidad } from './unidad';
import { UnidadesService } from './unidades.service';
import { IListaRetorno, Filtro } from '../generico/generico';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-unidades',
  templateUrl: './unidades.component.html',
  styleUrls: ['./unidades.component.css']
})
export class UnidadesComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  unidades: IUnidad[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';

  constructor(private unidadService: UnidadesService) { }

  ngOnInit() {
    this.getUnidades();
  }

  getUnidades() {
    this.unidadService.getUnidades({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IUnidad>) {
    this.unidades = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    console.log(this.pagina);
    this.getUnidades();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, this.busqueda));
      this.busqueda = '';
      this.getUnidades();
    }
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    }); 
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getUnidades();
  }
}
