import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";
import { IAlmacen } from "../almacenes/almacen";

export interface ISalidaProducto {
  idSalidaProducto: number;
  fechaCreacion: Date;
  usuario: IUsuario;
  comentario: string;
  almacen: IAlmacen;
  items: IItemSalidaProducto[];
}

export interface IItemSalidaProducto {
  idItemSalidaProducto?: number;
  salidaProducto?: ISalidaProducto;
  producto?: IProducto;
  unidad?: IUnidad;
  factor?: number;
  cantidad?: number;
}
