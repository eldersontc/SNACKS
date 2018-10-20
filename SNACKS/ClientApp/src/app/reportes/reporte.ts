export interface IReporte {
  idReporte: number;
  titulo: string;
  tipoReporte: number;
  flag: number;
  items: IItemReporte[];
}

export interface IItemReporte {
  idItemReporte: number;
  reporte: IReporte;
  nombre: string;
  valor: string;
}
