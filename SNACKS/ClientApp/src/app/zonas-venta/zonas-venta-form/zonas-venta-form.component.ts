import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ZonasVentaService } from '../zonas-venta.service';
import { Router, ActivatedRoute } from '@angular/router';
import { IZonaVenta } from '../zonaVenta';

@Component({
  selector: 'app-zonas-venta-form',
  templateUrl: './zonas-venta-form.component.html',
  styleUrls: ['./zonas-venta-form.component.css']
})
export class ZonasVentaFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private zonaVentaService: ZonasVentaService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idZonaVenta: 0,
      nombre: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.zonaVentaService.getZonaVenta(params["id"]).subscribe(zonaVenta => this.cargarFormulario(zonaVenta),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(zonaVenta: IZonaVenta) {
    this.form.patchValue({
      idZonaVenta: zonaVenta.idZonaVenta,
      nombre: zonaVenta.nombre
    });
  }

  save() {
    let zonaVenta: IZonaVenta = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.zonaVentaService.updateZonaVenta(zonaVenta)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.zonaVentaService.createZonaVenta(zonaVenta)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/zonas-venta"]);
  }

}
