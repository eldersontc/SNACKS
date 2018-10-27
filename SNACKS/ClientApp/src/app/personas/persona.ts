import { IZonaVenta } from "../zonas-venta/zonaVenta";

export interface IPersona {
  idPersona: number;
  tipoPersona: number;
  nombres: string;
  apellidos: string;
  razonSocial: string;
  tipoDocumento: number;
  numeroDocumento: string;
  direccion: string;
  distrito: string;
  zonaVenta: IZonaVenta;
  vendedor: IPersona;
}
