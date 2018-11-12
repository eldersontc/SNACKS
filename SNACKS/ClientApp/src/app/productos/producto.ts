import { IUnidad } from "../unidades/unidad";
import { ICategoria } from "../categorias/categoria";

export interface IProducto {
  idProducto?: number;
  nombre?: string,
  categoria?: ICategoria;
  esInsumo?: boolean;
  esProducto?: boolean;
  esGasto?: boolean;
  items?: IItemProducto[];
  insumos?: IInsumoProducto[];
}

export interface IItemProducto {
  idItemProducto?: number;
  producto?: IProducto;
  unidad?: IUnidad;
  factor?: number;
}

export interface IInsumoProducto {
  idInsumoProducto?: number;
  idProducto?: number;
  insumo?: IProducto;
}
