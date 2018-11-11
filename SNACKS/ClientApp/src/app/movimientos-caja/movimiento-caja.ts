import { IUsuario } from "../usuarios/usuario";

export interface IMovimientoCaja {
  idMovimientoCaja: number;
  idCaja: number;
  tipoMovimiento: string;
  idCajaOrigen: number;
  idPedido: number;
  idIngresoInsumo: number;
  glosa: number;
  fecha: Date;
  importe: number;
  usuario: IUsuario;
}
