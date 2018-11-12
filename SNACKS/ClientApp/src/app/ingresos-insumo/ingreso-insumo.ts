import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";
import { IAlmacen } from "../almacenes/almacen";
import { ICaja } from "../cajas/caja";

export interface IIngresoInsumo {
  idIngresoInsumo: number;
  fechaCreacion: Date;
  usuario: IUsuario;
  comentario: string;
  costo: number;
  almacen: IAlmacen,
  caja: ICaja,
  items: IItemIngresoInsumo[];
}

export interface IItemIngresoInsumo {
  idItemIngresoInsumo?: number;
  ingresoInsumo?: IIngresoInsumo;
  producto?: IProducto;
  unidad?: IUnidad;
  factor?: number;
  cantidad?: number;
  costo?: number;
}
