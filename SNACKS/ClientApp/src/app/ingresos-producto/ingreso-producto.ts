import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";

export interface IIngresoProducto {
  idIngresoProducto: number;
  fechaCreacion: Date;
  usuario: IUsuario;
  comentario: string;
  items: IItemIngresoProducto[];
}

export interface IItemIngresoProducto {
  idItemIngresoProducto: number;
  ingresoProducto: IIngresoProducto;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
  cantidad: number;
}
