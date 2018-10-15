import { IUnidad } from "../unidades/unidad";

export interface IProducto {
  idProducto: number;
  nombre: string,
  esInsumo: boolean;
  items: IItemProducto[];
}

export interface IItemProducto {
  idItemProducto: number;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
}
