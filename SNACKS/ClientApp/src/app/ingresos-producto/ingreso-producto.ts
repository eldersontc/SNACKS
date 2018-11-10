import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";
import { IAlmacen } from "../almacenes/almacen";

export interface IIngresoProducto {
  idIngresoProducto: number;
  fechaCreacion: Date;
  usuario: IUsuario;
  comentario: string;
  idLote: number;
  almacen: IAlmacen;
  items: IItemIngresoProducto[];
}

export interface IItemIngresoProducto {
  idItemIngresoProducto?: number;
  ingresoProducto?: IIngresoProducto;
  producto?: IProducto;
  unidad?: IUnidad;
  factor?: number;
  cantidad?: number;
}
