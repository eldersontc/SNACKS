import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule, NgbPaginationModule, NgbModalConfig, NgbModal  } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ClientesComponent } from './clientes/clientes.component';
import { UnidadesComponent } from './unidades/unidades.component';
import { UnidadesService } from './unidades/unidades.service';
import { UnidadesFormComponent } from './unidades/unidades-form/unidades-form.component';
import { PersonasComponent } from './personas/personas.component';
import { PersonasFormComponent } from './personas/personas-form/personas-form.component';
import { PersonasService } from './personas/personas.service';
import { ProductosComponent } from './productos/productos.component';
import { ProductosFormComponent } from './productos/productos-form/productos-form.component';
import { ProductosService } from './productos/productos.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ClientesComponent,
    UnidadesComponent,
    UnidadesFormComponent,
    PersonasComponent,
    PersonasFormComponent,
    ProductosComponent,
    ProductosFormComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AngularFontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'clientes', component: ClientesComponent },
      { path: 'unidades', component: UnidadesComponent },
      { path: 'unidades-form', component: UnidadesFormComponent },
      { path: 'unidades-form/:id', component: UnidadesFormComponent },
      { path: 'personas', component: PersonasComponent },
      { path: 'personas-form', component: PersonasFormComponent },
      { path: 'personas-form/:id', component: PersonasFormComponent },
      { path: 'productos', component: ProductosComponent },
      { path: 'productos-form', component: ProductosFormComponent },
      { path: 'productos-form/:id', component: ProductosFormComponent }
    ]),
    NgbModule,
    NgbPaginationModule
  ],
  providers: [
    NgbModalConfig,
    NgbModal,
    UnidadesService,
    PersonasService,
    ProductosService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
