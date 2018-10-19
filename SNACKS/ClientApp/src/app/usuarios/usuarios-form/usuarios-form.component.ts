import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UsuariosService } from '../usuarios.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IUsuario } from '../usuario';
import { IPersona } from '../../personas/persona';

@Component({
  selector: 'app-usuarios-form',
  templateUrl: './usuarios-form.component.html',
  styleUrls: ['./usuarios-form.component.css']
})
export class UsuariosFormComponent implements OnInit {

  elegirPersona: boolean = false;

  constructor(private fb: FormBuilder,
    private usuarioService: UsuariosService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idUsuario: 0,
      nombre: '',
      clave: '',
      persona: this.fb.group({
        idPersona: 0,
        razonSocial: '',
        nombres: '',
        apelllidos: ''
      })
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.usuarioService.getUsuario(params["id"]).subscribe(usuario => this.cargarFormulario(usuario),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(usuario: IUsuario) {
    this.form.patchValue({
      idUsuario: usuario.idUsuario,
      nombre: usuario.nombre,
      clave: usuario.clave,
      persona: {
        idPersona: usuario.persona.idPersona,
        razonSocial: (usuario.persona.tipoPersona == 2)
          ? usuario.persona.razonSocial : usuario.persona.apellidos + ', ' + usuario.persona.nombres
      }
    });
  }

  save() {
    let usuario: IUsuario = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.usuarioService.updateUsuario(usuario)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.usuarioService.createUsuario(usuario)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/usuarios"]);
  }

  buscarPersona() {
    this.elegirPersona = true;
  }

  asignarPersona(event: IPersona) {
    this.elegirPersona = false;
    if (event) {
      this.form.patchValue({
        persona: {
          idPersona: event.idPersona,
          razonSocial: (event.tipoPersona == 2)
            ? event.razonSocial : event.apellidos + ', ' + event.nombres
        }
      });
    }
  }
}
