import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IFiltro } from '../generico';

@Component({
  selector: 'app-filtros',
  templateUrl: './filtros.component.html',
  styleUrls: ['./filtros.component.css']
})
export class FiltrosComponent implements OnInit {

  @Input() filter: IFiltro[] = [];
  @Input() extern: IFiltro[] = [];

  @Output() change = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  quitarFiltro(f: IFiltro) {
    this.filter.forEach((item, index) => {
      if (item.k === f.k) this.filter.splice(index, 1);
    });
    this.change.emit();
  }

}
