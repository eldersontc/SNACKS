import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MovimientosCajaService } from '../movimientos-caja.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CajasService } from '../../cajas/cajas.service';
import { ICaja } from '../../cajas/caja';
import { IMovimientoCaja } from '../movimiento-caja';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';
import { ILogin } from '../../generico/generico';

@Component({
  selector: 'app-movimientos-caja-form',
  templateUrl: './movimientos-caja-form.component.html',
  styleUrls: ['./movimientos-caja-form.component.css']
})
export class MovimientosCajaFormComponent implements OnInit {

  caja: ICaja;
  nombreCaja: string;
  login: ILogin;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder,
    private movimientoCajaService: MovimientosCajaService,
    private cajaService: CajasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) {
    this.login = this.storage.get('login');
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.cajaService.getCaja(params["id"])
          .subscribe(caja => this.getCajaSuccess(caja),
          error => console.error(error));
      }
    });
  }

  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      glosa: '',
      importe: 0
    });
  }

  getCajaSuccess(caja: ICaja) {
    this.caja = caja;
    this.nombreCaja = caja.nombre;
  }

  save() {
    let movimientoCaja: IMovimientoCaja = Object.assign({}, this.form.value);

    movimientoCaja.idCaja = this.caja.idCaja;
    movimientoCaja.usuario = { idUsuario: this.login.id };

    this.movimientoCajaService.createMovimientoCaja(movimientoCaja)
      .subscribe(data => this.onSaveSuccess(),
        error => console.error(error));
  }

  onSaveSuccess() {
    this.router.navigate(["/cajas"]);
  }

}
