import { IUsuario } from "../usuarios/usuario";
import { IProducto } from "../productos/producto";

export interface ILote {
  idLote: number;
  fecha: Date,
  usuario: IUsuario;
  items: IItemLote[];
}

export interface IItemLote {
  idItemLote?: number;
  idLote?: number;
  producto?: IProducto;
}
