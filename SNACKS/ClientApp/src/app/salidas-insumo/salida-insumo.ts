import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";
import { IAlmacen } from "../almacenes/almacen";

export interface ISalidaInsumo {
  idSalidaInsumo: number;
  fechaCreacion: Date;
  usuario: IUsuario;
  almacen: IAlmacen;
  comentario: string;
  idLote: number;
  items: IItemSalidaInsumo[];
}

export interface IItemSalidaInsumo {
  idItemSalidaInsumo?: number;
  salidaInsumo?: ISalidaInsumo;
  producto?: IProducto;
  insumo?: IProducto;
  unidad?: IUnidad;
  factor?: number;
  cantidad?: number;
}
