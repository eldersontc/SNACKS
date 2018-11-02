import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule, NgbPaginationModule, NgbModalConfig, NgbModal, NgbDatepickerI18n, NgbDateAdapter, NgbDateParserFormatter  } from '@ng-bootstrap/ng-bootstrap';
import { StorageServiceModule } from 'angular-webstorage-service';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { UnidadesComponent } from './unidades/unidades.component';
import { UnidadesService } from './unidades/unidades.service';
import { UnidadesFormComponent } from './unidades/unidades-form/unidades-form.component';
import { PersonasComponent } from './personas/personas.component';
import { PersonasFormComponent } from './personas/personas-form/personas-form.component';
import { PersonasService } from './personas/personas.service';
import { ProductosComponent } from './productos/productos.component';
import { ProductosFormComponent } from './productos/productos-form/productos-form.component';
import { ProductosService } from './productos/productos.service';
import { PedidosComponent } from './pedidos/pedidos.component';
import { PedidosFormComponent } from './pedidos/pedidos-form/pedidos-form.component';
import { PedidosService } from './pedidos/pedidos.service';
import { DatepickerI18n, DateAdapter, DateParserFormatter } from './generico/generico.service';
import { CategoriasComponent } from './categorias/categorias.component';
import { CategoriasFormComponent } from './categorias/categorias-form/categorias-form.component';
import { CategoriasService } from './categorias/categorias.service';
import { IngresosInsumoComponent } from './ingresos-insumo/ingresos-insumo.component';
import { IngresosInsumoFormComponent } from './ingresos-insumo/ingresos-insumo-form/ingresos-insumo-form.component';
import { SalidasInsumoComponent } from './salidas-insumo/salidas-insumo.component';
import { SalidasInsumoFormComponent } from './salidas-insumo/salidas-insumo-form/salidas-insumo-form.component';
import { SalidasProductoComponent } from './salidas-producto/salidas-producto.component';
import { SalidasProductoFormComponent } from './salidas-producto/salidas-producto-form/salidas-producto-form.component';
import { IngresosProductoComponent } from './ingresos-producto/ingresos-producto.component';
import { IngresosProductoFormComponent } from './ingresos-producto/ingresos-producto-form/ingresos-producto-form.component';
import { IngresosInsumoService } from './ingresos-insumo/ingresos-insumo.service';
import { SalidasInsumoService } from './salidas-insumo/salidas-insumo.service';
import { IngresosProductoService } from './ingresos-producto/ingresos-producto.service';
import { SalidasProductoService } from './salidas-producto/salidas-producto.service';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { UsuariosFormComponent } from './usuarios/usuarios-form/usuarios-form.component';
import { UsuariosService } from './usuarios/usuarios.service';
import { ReportesComponent } from './reportes/reportes.component';
import { ReportesFormComponent } from './reportes/reportes-form/reportes-form.component';
import { ReportesService } from './reportes/reportes.service';
import { NotFoundComponent } from './not-found/not-found.component';
import { ZonasVentaComponent } from './zonas-venta/zonas-venta.component';
import { ZonasVentaFormComponent } from './zonas-venta/zonas-venta-form/zonas-venta-form.component';
import { ZonasVentaService } from './zonas-venta/zonas-venta.service';
import { FiltrosComponent } from './generico/filtros/filtros.component';
import { AccionesComponent } from './generico/acciones/acciones.component';
import { TablaComponent } from './generico/tabla/tabla.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    UnidadesComponent,
    UnidadesFormComponent,
    PersonasComponent,
    PersonasFormComponent,
    ProductosComponent,
    ProductosFormComponent,
    PedidosComponent,
    PedidosFormComponent,
    CategoriasComponent,
    CategoriasFormComponent,
    IngresosInsumoComponent,
    IngresosInsumoFormComponent,
    SalidasInsumoComponent,
    SalidasInsumoFormComponent,
    SalidasProductoComponent,
    SalidasProductoFormComponent,
    IngresosProductoComponent,
    IngresosProductoFormComponent,
    UsuariosComponent,
    UsuariosFormComponent,
    ReportesComponent,
    ReportesFormComponent,
    NotFoundComponent,
    ZonasVentaComponent,
    ZonasVentaFormComponent,
    FiltrosComponent,
    AccionesComponent,
    TablaComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    StorageServiceModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'usuarios', component: UsuariosComponent },
      { path: 'usuarios-form', component: UsuariosFormComponent },
      { path: 'usuarios-form/:id', component: UsuariosFormComponent },
      { path: 'unidades', component: UnidadesComponent },
      { path: 'unidades-form', component: UnidadesFormComponent },
      { path: 'unidades-form/:id', component: UnidadesFormComponent },
      { path: 'personas', component: PersonasComponent },
      { path: 'personas-form', component: PersonasFormComponent },
      { path: 'personas-form/:id', component: PersonasFormComponent },
      { path: 'categorias', component: CategoriasComponent },
      { path: 'categorias-form', component: CategoriasFormComponent },
      { path: 'categorias-form/:id', component: CategoriasFormComponent },
      { path: 'productos', component: ProductosComponent },
      { path: 'productos-form', component: ProductosFormComponent },
      { path: 'productos-form/:id', component: ProductosFormComponent },
      { path: 'pedidos', component: PedidosComponent },
      { path: 'pedidos-form', component: PedidosFormComponent },
      { path: 'pedidos-form/:id', component: PedidosFormComponent },
      { path: 'pedidos-form/:mode/:id', component: PedidosFormComponent },
      { path: 'ingresos-insumo', component: IngresosInsumoComponent },
      { path: 'ingresos-insumo-form', component: IngresosInsumoFormComponent },
      { path: 'ingresos-insumo-form/:id', component: IngresosInsumoFormComponent },
      { path: 'salidas-insumo', component: SalidasInsumoComponent },
      { path: 'salidas-insumo-form', component: SalidasInsumoFormComponent },
      { path: 'salidas-insumo-form/:id', component: SalidasInsumoFormComponent },
      { path: 'ingresos-producto', component: IngresosProductoComponent },
      { path: 'ingresos-producto-form', component: IngresosProductoFormComponent },
      { path: 'ingresos-producto-form/:id', component: IngresosProductoFormComponent },
      { path: 'salidas-producto', component: SalidasProductoComponent },
      { path: 'salidas-producto-form', component: SalidasProductoFormComponent },
      { path: 'salidas-producto-form/:id', component: SalidasProductoFormComponent },
      { path: 'reportes', component: ReportesComponent },
      { path: 'reportes-form', component: ReportesFormComponent },
      { path: 'reportes-form/:id', component: ReportesFormComponent },
      { path: 'zonas-venta', component: ZonasVentaComponent },
      { path: 'zonas-venta-form', component: ZonasVentaFormComponent },
      { path: 'zonas-venta-form/:id', component: ZonasVentaFormComponent },
      { path: '404', component: NotFoundComponent },
      { path: '**', redirectTo: '/404' }
    ]),
    NgbModule,
    NgbPaginationModule
  ],
  providers: [
    NgbModalConfig,
    NgbModal,
    UsuariosService,
    UnidadesService,
    PersonasService,
    CategoriasService,
    ProductosService,
    PedidosService,
    IngresosInsumoService,
    SalidasInsumoService,
    IngresosProductoService,
    SalidasProductoService,
    ReportesService,
    ZonasVentaService,
    { provide: NgbDatepickerI18n, useClass: DatepickerI18n },
    { provide: NgbDateAdapter, useClass: DateAdapter },
    { provide: NgbDateParserFormatter, useClass: DateParserFormatter }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
