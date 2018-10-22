import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UsuariosService } from './usuarios/usuarios.service';
import { IUsuario } from './usuarios/usuario';
import { LOCAL_STORAGE, WebStorageService } from 'angular-webstorage-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  logueado: boolean = false;

  constructor(@Inject(LOCAL_STORAGE) private storage: WebStorageService,
    private fb: FormBuilder, private usuarioService: UsuariosService) { }

  form: FormGroup;

  verAlerta: boolean = false;

  ngOnInit() {
    this.form = this.fb.group({
      nombre: '',
      clave: ''
    });
    if (this.storage.get('login')) {
      this.logueado = true;
      this.usuarioLogueado = this.storage.get('login').nombre;
    }
  }

  auth() {
    let usuario: IUsuario = Object.assign({}, this.form.value);

    this.usuarioService.authUsuario(usuario).subscribe(u => this.onAuthSuccess(u),
      error => this.verAlerta = true);
  }

  onAuthSuccess(u: IUsuario) {
    this.logueado = true;
    this.storage.set('login', {
      id: u.idUsuario,
      nombre: u.persona.tipoPersona == 2
        ? u.persona.razonSocial : u.persona.nombres + ' ' + u.persona.apellidos,
      tipo: u.persona.tipoPersona
    });
    this.usuarioLogueado = this.storage.get('login').nombre;
  }

  cerrarSesion() {
    this.logueado = false;
    this.storage.remove('login');
  }

  usuarioLogueado: string;

  opciones: Object[] = [
    {
      nombre: 'Ventas', icono: 'shopping-cart',
      subOpciones: [
        { nombre: 'Pedidos', icono: 'clipboard', link: 'pedidos' }
      ]
    },
    {
      nombre: 'Producción', icono: 'industry',
      subOpciones: [
        { nombre: 'Unidades', icono: 'archive', link: 'unidades' },
        { nombre: 'Categorias', icono: 'archive', link: 'categorias' },
        { nombre: 'Productos', icono: 'archive', link: 'productos' },
        { nombre: 'Ingresos Insumo', icono: 'file-import', link: 'ingresos-insumo' },
        { nombre: 'Salidas Insumo', icono: 'file-export', link: 'salidas-insumo' },
        { nombre: 'Ingresos Producto', icono: 'file-import', link: 'ingresos-producto' },
        { nombre: 'Salidas Producto', icono: 'file-export', link: 'salidas-producto' }
      ]
    },
    {
      nombre: 'Gerencia', icono: 'chart-bar',
      subOpciones: [
        { nombre: 'Personas', icono: 'user-circle', link: 'personas' },
        { nombre: 'Usuarios', icono: 'user-circle', link: 'usuarios' },
        { nombre: 'Reportes', icono: 'chart-pie', link: 'reportes' }
      ]
    }
  ];

}
