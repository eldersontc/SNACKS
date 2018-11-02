export interface IListaRetorno<T> {
  lista: T[];
  totalRegistros: number;
}

export interface IEstadistica {
  leyenda: string;
  etiqueta: string;
  valor: number;
}

export interface IFiltro {
  //constructor(
  //  public k: number,
  //  public v: string,
  //  public n: number = 1,
  //  public d: Date = new Date(),
  //  public b: boolean = false
  //) { }
  k: number;
  v?: string;
  n?: number;
  d?: Date;
  b?: boolean;
}

export interface ILogin {
  id: number;
  nombre: string;
  idPersona: number;
  nombrePersona: string;
  tipo: number;
}
