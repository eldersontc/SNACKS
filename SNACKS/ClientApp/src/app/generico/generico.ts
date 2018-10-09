export interface IListaRetorno<T> {
  lista: T[];
  totalRegistros: number;
}

export class Filtro {
  constructor(
    public k: number,
    public v: string,
    public n: number = 1,
    public d: string = '01/01/2019'
  ) { }
}
