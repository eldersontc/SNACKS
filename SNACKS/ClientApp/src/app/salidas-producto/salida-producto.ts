import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";

export interface ISalidaProducto {
  idSalidaProducto: number;
  fechaCreacion: Date;
  comentario: string;
  items: IItemSalidaProducto[];
}

export interface IItemSalidaProducto {
  idItemSalidaProducto: number;
  salidaProducto: ISalidaProducto;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
  cantidad: number;
}
