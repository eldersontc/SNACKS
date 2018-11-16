import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { IItemReporte, IReporte } from '../reporte';
import { ReportesService } from '../reportes.service';

@Component({
  selector: 'app-reportes-form',
  templateUrl: './reportes-form.component.html',
  styleUrls: ['./reportes-form.component.css']
})
export class ReportesFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private reporteService: ReportesService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;
  formItem: FormGroup;

  items: IItemReporte[] = [];

  tipos = ['bar', 'pie', 'line','doughnut','table'];

  ngOnInit() {
    this.form = this.fb.group({
      idReporte: 0,
      titulo: '',
      tipoReporte: 'bar',
      flag: ''
    });
    this.formItem = this.fb.group({
      nombre: '',
      valor: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.reporteService.getReporte(params["id"])
          .subscribe(reporte => this.cargarFormulario(reporte),
            error => console.error(error));
      }
    });
  }

  cargarFormulario(reporte: IReporte) {
    this.form.patchValue({
      idReporte: reporte.idReporte,
      titulo: reporte.titulo,
      tipoReporte: reporte.tipoReporte,
      flag: reporte.flag
    });
    this.items = reporte.items;
  }

  save() {
    let reporte: IReporte = Object.assign({}, this.form.value);

    reporte.items = this.items;

    if (this.modoEdicion) {
      this.reporteService.updateReporte(reporte)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.reporteService.createReporte(reporte)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/reportes"]);
  }

  saveItem() {
    let i: IItemReporte = Object.assign({}, this.formItem.value);

    //if (this.modoEdicion) {

    //  let reporte: IReporte = Object.assign({}, this.form.value);
    //  i.reporte = reporte;

    //  this.reporteService.createItem(i)
    //    .subscribe(data => this.onSaveItemSuccess(i),
    //      error => console.error(error));

    //} else {
      this.onSaveItemSuccess(i);
    //}
  }

  onSaveItemSuccess(i) {
    this.items.push(i);
    this.formItem.reset();
  }

  deleteItem(i: IItemReporte) {
    //if (this.modoEdicion) {
    //  this.reporteService.deleteItem(i.idItemReporte)
    //    .subscribe(data => this.onDeleteItemSuccess(i),
    //      error => console.log(error));
    //} else {
      this.onDeleteItemSuccess(i);
    //}
  }

  onDeleteItemSuccess(i: IItemReporte) {
    this.items.forEach((item, index) => {
      if (item.idItemReporte === i.idItemReporte) this.items.splice(index, 1);
    });
  }

}
