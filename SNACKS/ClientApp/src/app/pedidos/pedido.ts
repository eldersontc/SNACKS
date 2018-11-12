import { IPersona } from "../personas/persona";
import { IUnidad } from "../unidades/unidad";
import { IProducto } from "../productos/producto";
import { IUsuario } from "../usuarios/usuario";

export interface IPedido {
  idPedido: number;
  cliente: IPersona;
  fechaCreacion: Date;
  fechaPago: Date;
  fechaPropuesta: Date;
  fechaEntrega: Date;
  usuario: IUsuario;
  estado: string;
  comentario: string;
  total: number;
  pago: number;
  items: IItemPedido[];
}

export interface IItemPedido {
  idItemPedido?: number;
  pedido?: IPedido;
  producto?: IProducto;
  unidad?: IUnidad;
  factor?: number;
  cantidad?: number;
  total?: number;
}
