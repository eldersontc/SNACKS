import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  logueado: string = 'Elderson Taboada';
  opcion: object;
  subOpcion: object;

  opciones: Object[] = [
    {
      nombre: 'Ventas', icono: 'shopping-cart',
      subOpciones: [
        { nombre: 'Clientes', icono: 'users', link: 'clientes' },
        { nombre: 'Vendedores', icono: 'user', link: 'vendedores' },
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
        { nombre: 'Salidas Producto', icono: 'file-export', link: 'salidas-producto' },
        { nombre: 'Gastos', icono: 'file-invoice-dollar' }
      ]
    },
    {
      nombre: 'Gerencia', icono: 'chart-bar',
      subOpciones: [
        { nombre: 'Personas', icono: 'user-circle', link: 'personas' },
        { nombre: 'Ususarios', icono: 'user-circle', link: 'usuarios' },
        { nombre: 'Perfiles', icono: 'users-cog' },
        { nombre: 'Reportes', icono: 'chart-pie' }
      ]
    }
  ];

  seleccion(opcion, subOpcion) {
    this.opcion = opcion;
    this.subOpcion = subOpcion;
  }

}
