import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AlmacenesService } from '../almacenes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IAlmacen } from '../almacen';

@Component({
  selector: 'app-almacenes-form',
  templateUrl: './almacenes-form.component.html',
  styleUrls: ['./almacenes-form.component.css']
})
export class AlmacenesFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private almacenService: AlmacenesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idAlmacen: 0,
      nombre: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.almacenService.getAlmacen(params["id"]).subscribe(almacen => this.cargarFormulario(almacen),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(almacen: IAlmacen) {
    this.form.patchValue({
      idAlmacen: almacen.idAlmacen,
      nombre: almacen.nombre
    });
  }

  save() {
    let almacen: IAlmacen = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.almacenService.updateAlmacen(almacen)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.almacenService.createAlmacen(almacen)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/almacenes"]);
  }

}
