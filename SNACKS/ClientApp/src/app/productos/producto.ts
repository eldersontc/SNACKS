import { IUnidad } from "../unidades/unidad";
import { ICategoria } from "../categorias/categoria";

export interface IProducto {
  idProducto: number;
  nombre: string,
  categoria: ICategoria;
  esInsumo: boolean;
  items: IItemProducto[];
}

export interface IItemProducto {
  idItemProducto: number;
  producto: IProducto;
  unidad: IUnidad;
  factor: number;
}
