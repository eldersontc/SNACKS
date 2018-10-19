import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IUsuario } from './usuario';
import { Filtro, IListaRetorno } from '../generico/generico';
import { UsuariosService } from './usuarios.service';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {

  @Input() modo: number;
  @Output() model = new EventEmitter();

  pagina: number = 1;
  totalRegistros: number = 0;
  usuarios: IUsuario[];
  filtros: Filtro[] = [];
  criterio: number = 1;
  busqueda: string = '';
  seleccion: IUsuario;

  constructor(private unidadService: UsuariosService, config: NgbModalConfig, private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
    this.getUsuarios();
  }

  getUsuarios() {
    this.unidadService.getUsuarios({
      "Registros": 10,
      "Pagina": this.pagina,
      "filtros": this.filtros
    })
      .subscribe(data => this.onGetSuccess(data),
        error => console.error(error));
  }

  onGetSuccess(data: IListaRetorno<IUsuario>) {
    this.usuarios = data.lista;
    this.totalRegistros = data.totalRegistros
  }

  pageChange() {
    this.seleccion = undefined;
    this.getUsuarios();
  }

  buscar() {
    if (this.busqueda.length > 0) {
      this.quitarCriterio(this.criterio);
      this.filtros.push(new Filtro(this.criterio, this.busqueda));
      this.busqueda = '';
      this.getUsuarios();
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
    this.getUsuarios();
  }

  open(content) {
    this.modalService.open(content, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.deleteUsuario(); } });
  }

  deleteUsuario() {
    this.unidadService.deleteUsuario(this.seleccion.idUsuario).subscribe(data => this.onDeleteSuccess(),
      error => console.log(error));
  }

  onDeleteSuccess() {
    this.seleccion = undefined;
    this.getUsuarios();
  }

  elegir() {
    this.model.emit(this.seleccion);
  }

  cancelar() {
    this.model.emit();
  }

}
