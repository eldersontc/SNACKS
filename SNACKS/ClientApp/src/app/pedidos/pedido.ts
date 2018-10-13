import { IPersona } from "../personas/persona";

export interface IPedido {
  idPedido: number;
  cliente: IPersona;
  fechaCreacion: Date;
}
