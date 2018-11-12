import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { UsuariosService } from './usuarios/usuarios.service';
import { IUsuario } from './usuarios/usuario';
import { ILogin } from './generico/generico';
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
  opciones: object;

  ngOnInit() {
    this.form = this.fb.group({
      nombre: '',
      clave: ''
    });
    let login: ILogin = Object.assign({}, this.storage.get('login'));
    if (login) {
      this.logueado = true;
      this.usuarioLogueado = this.getUserLogin(login);
      this.opciones = this.opcionesxRol(login.tipo);
    }
  }

  auth() {
    let usuario: IUsuario = Object.assign({}, this.form.value);

    this.usuarioService.authUsuario(usuario).subscribe(u => this.onAuthSuccess(u),
      error => this.verAlerta = true);
  }

  getUserLogin(login) {
    var rol;
    switch (login.tipo) {
      case 1:
        rol = 'Gerente';
        break;
      case 2:
        rol = 'Cliente';
        break;
      case 3:
        rol = 'Vendedor';
        break;
      case 4:
        rol = 'Empleado';
        break;
    }
    return login.nombrePersona + ' | ' + rol;
  }

  onAuthSuccess(u: IUsuario) {
    this.logueado = true;
    this.storage.set('login', {
      id: u.idUsuario,
      nombre: u.nombre,
      idPersona: u.persona.idPersona,
      nombrePersona: u.persona.tipoPersona == 2
        ? u.persona.razonSocial : u.persona.nombres + ' ' + u.persona.apellidos,
      tipo: u.persona.tipoPersona
    });
    this.usuarioLogueado = this.getUserLogin(this.storage.get('login'));
    this.opciones = this.opcionesxRol(this.storage.get('login').tipo);
  }

  cerrarSesion() {
    this.logueado = false;
    this.storage.remove('login');
  }

  usuarioLogueado: string;

  opcionesxRol(rol) {
    var opciones;
    if (rol == 1) {
      opciones = [
        {
          nombre: 'Ventas', icono: 'shopping-cart',
          subOpciones: [
            { nombre: 'Pedidos', icono: 'clipboard', link: 'pedidos' },
            { nombre: 'Zonas Venta', icono: 'clipboard', link: 'zonas-venta' }
          ]
        },
        {
          nombre: 'Producción', icono: 'industry',
          subOpciones: [
            { nombre: 'Almacenes', icono: 'archive', link: 'almacenes' },
            { nombre: 'Unidades', icono: 'archive', link: 'unidades' },
            { nombre: 'Categorias', icono: 'archive', link: 'categorias' },
            { nombre: 'Productos', icono: 'archive', link: 'productos' },
            { nombre: 'Lotes', icono: 'archive', link: 'lotes' },
            { nombre: 'Ingresos Insumo', icono: 'file-import', link: 'ingresos-insumo' },
            { nombre: 'Salidas Insumo', icono: 'file-export', link: 'salidas-insumo' },
            { nombre: 'Ingresos Producto', icono: 'file-import', link: 'ingresos-producto' },
            { nombre: 'Salidas Producto', icono: 'file-export', link: 'salidas-producto' }
          ]
        },
        {
          nombre: 'Gerencia', icono: 'chart-bar',
          subOpciones: [
            { nombre: 'Cajas', icono: 'user-circle', link: 'cajas' },
            { nombre: 'Personas', icono: 'user-circle', link: 'personas' },
            { nombre: 'Usuarios', icono: 'user-circle', link: 'usuarios' },
            { nombre: 'Reportes', icono: 'chart-pie', link: 'reportes' }
          ]
        }
      ];
    }

    if (rol == 2 || rol == 3) {
      opciones = [
        {
          nombre: 'Ventas', icono: 'shopping-cart',
          subOpciones: [
            { nombre: 'Pedidos', icono: 'clipboard', link: 'pedidos' }
          ]
        }
      ];
    }

    if (rol == 4) {
      opciones = [
        {
          nombre: 'Producción', icono: 'industry',
          subOpciones: [
            { nombre: 'Lotes', icono: 'archive', link: 'lotes' },
            { nombre: 'Ingresos Insumo', icono: 'file-import', link: 'ingresos-insumo' },
            { nombre: 'Salidas Insumo', icono: 'file-export', link: 'salidas-insumo' },
            { nombre: 'Ingresos Producto', icono: 'file-import', link: 'ingresos-producto' },
            { nombre: 'Salidas Producto', icono: 'file-export', link: 'salidas-producto' }
          ]
        }
      ];
    }

    return opciones;
  }

  

}
