import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { UnidadesService } from '../unidades.service';
import { Router } from '@angular/router';
import { IUnidad } from '../unidad';

@Component({
  selector: 'app-unidades-form',
  templateUrl: './unidades-form.component.html',
  styleUrls: ['./unidades-form.component.css']
})
export class UnidadesFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private unidadService: UnidadesService,
    private router: Router) { }

  formGroup: FormGroup;

  ngOnInit() {
    this.formGroup = this.fb.group({
      abreviacion: '',
      nombre: ''
    });
  }

  save() {
    let unidad: IUnidad = Object.assign({}, this.formGroup.value);

    this.unidadService.createUnidad(unidad)
      .subscribe(data => this.onSaveSuccess(),
        error => console.error(error));
  }

  onSaveSuccess() {
    this.router.navigate(["/unidades"]);
  }
}
