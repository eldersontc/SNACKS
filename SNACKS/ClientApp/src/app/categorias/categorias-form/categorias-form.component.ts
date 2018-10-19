import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CategoriasService } from '../categorias.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ICategoria } from '../categoria';

@Component({
  selector: 'app-categorias-form',
  templateUrl: './categorias-form.component.html',
  styleUrls: ['./categorias-form.component.css']
})
export class CategoriasFormComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private categoriaService: CategoriasService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  modoEdicion: boolean;
  form: FormGroup;

  ngOnInit() {
    this.form = this.fb.group({
      idCategoria: 0,
      nombre: ''
    });
    this.activatedRoute.params.subscribe(params => {
      if (params["id"] == undefined) {
        return;
      } else {
        this.modoEdicion = true;
        this.categoriaService.getCategoria(params["id"]).subscribe(categoria => this.cargarFormulario(categoria),
          error => console.error(error));
      }
    });
  }

  cargarFormulario(categoria: ICategoria) {
    this.form.patchValue({
      idCategoria: categoria.idCategoria,
      nombre: categoria.nombre
    });
  }

  save() {
    let categoria: ICategoria = Object.assign({}, this.form.value);

    if (this.modoEdicion) {
      this.categoriaService.updateCategoria(categoria)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    } else {
      this.categoriaService.createCategoria(categoria)
        .subscribe(data => this.onSaveSuccess(),
          error => console.error(error));
    }
  }

  onSaveSuccess() {
    this.router.navigate(["/categorias"]);
  }
}
