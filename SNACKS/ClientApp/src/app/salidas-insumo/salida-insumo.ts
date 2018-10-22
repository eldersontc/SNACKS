import { IProducto } from "../productos/producto";
import { IUnidad } from "../unidades/unidad";
import { IUsuario } from "../usuarios/usuario";

export interface ISalidaInsumo {
  idSalidaInsumo: number;
  fechaCreacion: Date;
  usuario: IUsuario;
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
