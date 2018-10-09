import { Component, OnInit } from '@angular/core';
import { IPersona } from './persona';
import { Filtro, IListaRetorno } from '../generico/generico';
import { PersonasService } from './personas.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-personas',
  templateUrl: './personas.component.html',
  styleUrls: ['./personas.component.css']
})
export class PersonasComponent implements OnInit {

  pagina: number = 1;
  totalRegistros: number = 0;
  personas: IPersona[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  busquedaCombo: string = '1-Gerente';
  seleccion: number;

  constructor(private personaService: PersonasService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getPersonas();
  }

  getPersonas() {
    this.personaService.getPersonas({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IPersona>) {
    this.personas = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getPersonas();
  }

  buscar() {
    if (this.criterio == 1) {
      this.quitarCriterio(this.criterio);
      const arr: string[] = this.busquedaCombo.split('-');
      this.filtros.push(new Filtro(this.criterio, arr[1], Number(arr[0])));
      this.getPersonas();
    } else {
      if (this.busqueda.length > 0) {
        this.quitarCriterio(this.criterio);
        this.filtros.push(new Filtro(this.criterio, this.busqueda));
        this.busqueda = '';
        this.getPersonas();
      }
    }
  }

  quitarCriterio(k: number) {
    this.filtros.forEach((item, index) => {
      if (item.k === k) this.filtros.splice(index, 1);
    });
    this.seleccion = undefined;
  }

  quitarFiltro(filtro: Filtro) {
    this.quitarCriterio(filtro.k);
    this.getPersonas();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deletePersona(); } });
  }

  deletePersona() {
    this.personaService.deletePersona(this.seleccion).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getPersonas();
  }
}
