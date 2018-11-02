import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PersonasService } from '../personas.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IPersona } from '../persona';
import { IZonaVenta } from '../../zonas-venta/zonaVenta';
import { IFiltro } from '../../generico/generico';

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

  filtrosVendedor: IFiltro[] = [];
  elegirVendedor: boolean = false;
  elegirZonaVenta: boolean = false;
  modoEdicion: boolean;
  form: FormGroup;

  distritos =
    [
      'Ancon',
      'Ate',
      'Barranco',
      'Breña',
      'Carabayllo',
      'Cercado',
      'Chaclacayo',
      'Chorrillos',
      'Cieneguilla',
      'Comas',
      'El Agustino',
      'Independencia',
      'Jesus Maria',
      'La Molina',
      'La Victoria',
      'Lima',
      'Lince',
      'Los Olivos',
      'Lurigancho',
      'Lurin',
      'Magdalena',
      'Miraflores',
      'Pachacamac',
      'Pucusana',
      'Pueblo Libre',
      'Puente Piedra',
      'Punta Hermosa',
      'Punta Negra',
      'Rimac',
      'San Bartolo',
      'San Borja',
      'San Isidro',
      'San Juan De Lurigancho',
      'San Juan De Miraflores',
      'San Luis',
      'San Martin De Porres',
      'San Miguel',
      'Santa Anita',
      'Santa Maria Del Mar',
      'Santa Rosa',
      'Santiago De Surco',
      'Surquillo',
      'Villa El Salvador',
      'Villa Maria Del Triunfo'
    ];

  ngOnInit() {
    this.filtrosVendedor.push({ k: 1, v: 'Vendedor', n: 3 });
    this.form = this.fb.group({
      idPersona: 0,
      tipoPersona: 1,
      nombres: '',
      apellidos: '',
      razonSocial: '',
      tipoDocumento: 1,
      numeroDocumento: '',
      distrito: 'Lima',
      direccion: '',
      zonaVenta: this.fb.group({
        idZonaVenta: 0,
        nombre: ''
      }),
      vendedor: this.fb.group({
        idPersona: 0,
        razonSocial: ''
      })
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
    this.form.patchValue({
      idPersona: persona.idPersona,
      tipoPersona: persona.tipoPersona,
      nombres: persona.nombres,
      apellidos: persona.apellidos,
      razonSocial: persona.razonSocial,
      tipoDocumento: persona.tipoDocumento,
      numeroDocumento: persona.numeroDocumento,
      direccion: persona.direccion,
      distrito: persona.distrito
    });
    if (persona.tipoPersona == 2) {
      this.form.patchValue({
        zonaVenta: persona.zonaVenta,
        vendedor: {
          idPersona: persona.vendedor.idPersona,
          razonSocial: persona.vendedor.apellidos + ', ' + persona.vendedor.nombres
        }
      })
    }
  }

  save() {
    let persona: IPersona = Object.assign({}, this.form.value);

    if (persona.tipoPersona != 2) {
      persona.zonaVenta = undefined;
      persona.vendedor = undefined;
    }

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

  buscarZonaVenta() {
    this.elegirZonaVenta = true;
  }

  asignarZonaVenta(event: IZonaVenta) {
    this.elegirZonaVenta = false;
    if (event) {
      this.form.patchValue({
        zonaVenta: event
      });
    }
  }

  buscarVendedor() {
    this.elegirVendedor = true;
  }

  asignarVendedor(event: IPersona) {
    this.elegirVendedor = false;
    if (event) {
      this.form.patchValue({
        vendedor: {
          idPersona: event.idPersona,
          razonSocial: event.apellidos + ', ' + event.nombres
        }
      });
    }
  }
}
