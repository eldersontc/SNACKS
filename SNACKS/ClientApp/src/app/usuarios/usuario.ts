import { IPersona } from "../personas/persona";

export interface IUsuario {
  idUsuario: number;
  nombre?: string;
  clave?: string;
  persona?: IPersona;
}
