import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";

export interface ISalidaInsumo {
  idSalidaInsumo: number;
  fechaCreacion: Date;
  comentario: string;
  items: IItemSalidaInsumo[];
}

export interface IItemSalidaInsumo {
  idItemSalidaInsumo: number;
  salidaInsumo: ISalidaInsumo;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
  cantidad: number;
}
