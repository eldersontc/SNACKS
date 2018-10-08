export interface IListaRetorno<T> {
  lista: T[];
  totalRegistros: number;
}

export class Filtro {
  constructor(
    public k: number,
    public v: string/*,
    private n: number = 1,
    private d: string = ''*/
  ) { }
}
