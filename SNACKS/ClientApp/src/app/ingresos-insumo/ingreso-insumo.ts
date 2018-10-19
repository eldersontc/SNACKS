import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";

export interface IIngresoInsumo {
  idIngresoInsumo: number;
  fechaCreacion: Date;
  comentario: string;
  costo: number;
  items: IItemIngresoInsumo[];
}

export interface IItemIngresoInsumo {
  idItemIngresoInsumo: number;
  ingresoInsumo: IIngresoInsumo;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
  cantidad: number;
  costo: number;
}
