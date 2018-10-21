import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";

export interface IIngresoProducto {
  idIngresoProducto: number;
  fechaCreacion: Date;
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
