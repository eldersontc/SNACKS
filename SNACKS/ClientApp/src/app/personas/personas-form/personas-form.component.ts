import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PersonasService } from '../personas.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPersona } from '../persona';

@Component({
  selector: 'app-personas-form',
  templateUrl: './personas-form.component.html',
  styleUrls: ['./personas-form.component.css']
})
export class PersonasFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private personaService: PersonasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  formGroup: FormGroup;

  ngOnInit() {
    this.formGroup = this.fb.group({
      idPersona: 0,
      tipoPersona: 1,
      nombres: '',
      apellidos: '',
      razonSocial: '',
      numeroDocumento: '',
      direccion: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.personaService.getPersona(params["id"]).subscribe(unidad => this.cargarFormulario(unidad),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(persona: IPersona) {
    this.formGroup.patchValue({
      idPersona: persona.idPersona,
      tipoPersona: persona.tipoPersona,
      nombres: persona.nombres,
      apellidos: persona.apellidos,
      razonSocial: persona.razonSocial,
      numeroDocumento: persona.numeroDocumento,
      direccion: persona.direccion
    });
  }

  save() {
    let persona: IPersona = Object.assign({}, this.formGroup.value);

    if (this.modoEdicion) {
      this.personaService.updatePersona(persona)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.personaService.createPersona(persona)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/personas"]);
  }

}
