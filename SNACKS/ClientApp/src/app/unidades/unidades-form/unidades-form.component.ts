import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UnidadesService } from '../unidades.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IUnidad } from '../unidad';

@Component({
  selector: 'app-unidades-form',
  templateUrl: './unidades-form.component.html',
  styleUrls: ['./unidades-form.component.css']
})
export class UnidadesFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private unidadService: UnidadesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idUnidad: 0,
      abreviacion: '',
      nombre: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.unidadService.getUnidad(params["id"]).subscribe(unidad => this.cargarFormulario(unidad),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(unidad: IUnidad) {
    this.form.patchValue({
      idUnidad: unidad.idUnidad,
      nombre: unidad.nombre,
      abreviacion: unidad.abreviacion
    });
  }

  save() {
    let unidad: IUnidad = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.unidadService.updateUnidad(unidad)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.unidadService.createUnidad(unidad)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/unidades"]);
  }
}
