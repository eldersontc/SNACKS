import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  @Input() opciones: Object[];
  @Input() logueado: string;
  @Output() salir = new EventEmitter();

  cerrarSesion() {
    this.salir.emit();
  }

}
