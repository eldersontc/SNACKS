import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  logueado = 'Elderson Taboada';

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
        { nombre: 'Productos', icono: 'archive' },
        { nombre: 'Ingresos', icono: 'file-import' },
        { nombre: 'Salidas', icono: 'file-export' },
        { nombre: 'Gastos', icono: 'file-invoice-dollar' }
      ]
    },
    {
      nombre: 'Gerencia', icono: 'chart-bar',
      subOpciones: [
        { nombre: 'Ususarios', icono: 'user-circle' },
        { nombre: 'Perfiles', icono: 'users-cog' },
        { nombre: 'Reportes', icono: 'chart-pie' }
      ]
    }
  ];

}
