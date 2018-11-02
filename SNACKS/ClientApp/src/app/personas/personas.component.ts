import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IPersona } from './persona';
import { IFiltro, IListaRetorno } from '../generico/generico';
import { PersonasService } from './personas.service';

@Component({
  selector: 'app-personas',
  templateUrl: './personas.component.html',
  styleUrls: ['./personas.component.css']
})
export class PersonasComponent implements OnInit {

  @Input() extern: IFiltro[];
  @Output() select = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  personas: IPersona[];
  filtros: IFiltro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  busquedaCombo: string = '1-Gerente';
  seleccion: IPersona;

  columnas: string[][] = [
    ['L','Razón Social'],
    ['L','Nombres'],
    ['L','Apellidos'],
    ['L','Nro. Documento']];
  atributos: string[][] = [
    ['S', 'L','razonSocial'],
    ['S', 'L','nombres'],
    ['S', 'L','apellidos'],
    ['S', 'L','numeroDocumento']]

  constructor(private personaService: PersonasService) { }

  ngOnInit() {
    if (this.extern) {
      this.criterio = 2;
    }
    this.getPersonas();
  }

  getPersonas() {
    this.seleccion = undefined;
    this.personaService.getPersonas({
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

  onGetSuccess(data: IListaRetorno<IPersona>) {
    this.personas = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.getPersonas();
  }

  buscar() {
    if (this.criterio == 1) {
      const arr: string[] = this.busquedaCombo.split('-');
      this.filtros.push({ k: this.criterio, v: arr[1], n: Number(arr[0]) });
      this.getPersonas();
    } else {
      if (this.busqueda.length > 0) {
        this.filtros.push({ k: this.criterio, v: this.busqueda });
        this.busqueda = '';
        this.getPersonas();
      }
    }
  }
 
  deletePersona() {
    this.personaService.deletePersona(this.seleccion.idPersona).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getPersonas();
  }

  elegir() {
    this.select.emit(this.seleccion);
  }

  cancelar() {
    this.select.emit();
  }
}
