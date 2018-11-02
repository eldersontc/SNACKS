import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModalConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-acciones',
  templateUrl: './acciones.component.html',
  styleUrls: ['./acciones.component.css']
})
export class AccionesComponent implements OnInit {

  @Input() link: string;
  @Input() class: string;
  @Input() model: object;
  @Input() modeSearch: boolean;
  @Input() show: boolean = true;
  @Input() showReadOnly: boolean = false;

  @Output() delete = new EventEmitter();
  @Output() select = new EventEmitter();
  @Output() cancel = new EventEmitter();

  constructor(private router: Router,
    config: NgbModalConfig,
    private modalService: NgbModal) {
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit() {
  }

  nuevo() {
    this.router.navigate(['/' + this.link]);
  }

  editar() {
    this.router.navigate(['/' + this.link + '/' + this.model['id' + this.class]]);
  }

  open(confirm) {
    this.modalService.open(confirm, { centered: true, size: 'sm' })
      .result.then((result) => { if (result == 'Eliminar') { this.delete.emit(); } });
  }

  seleccionar() {
    this.select.emit();
  }

  cancelar() {
    this.cancel.emit();
  }

  ver() {
    this.router.navigate(['/' + this.link + '/~/' + this.model['id' + this.class]]);
  }
}
