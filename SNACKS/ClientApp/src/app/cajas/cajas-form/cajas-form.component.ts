import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CajasService } from '../cajas.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ICaja } from '../caja';

@Component({
  selector: 'app-cajas-form',
  templateUrl: './cajas-form.component.html',
  styleUrls: ['./cajas-form.component.css']
})
export class CajasFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private cajaService: CajasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idCaja: 0,
      nombre: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.cajaService.getCaja(params["id"]).subscribe(caja => this.cargarFormulario(caja),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(caja: ICaja) {
    this.form.patchValue({
      idCaja: caja.idCaja,
      nombre: caja.nombre
    });
  }

  save() {
    let caja: ICaja = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.cajaService.updateCaja(caja)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.cajaService.createCaja(caja)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/cajas"]);
  }

}
