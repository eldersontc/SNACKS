import { Injectable } from "@angular/core";
import { NgbDatepickerI18n, NgbDateStruct, NgbDateParserFormatter, NgbDateAdapter } from "@ng-bootstrap/ng-bootstrap";

export interface IListaRetorno<T> {
  lista: T[];
  totalRegistros: number;
}

export interface IEstadistica {
  leyenda: string;
  etiqueta: string;
  valor: number;
}

export class Filtro {
  constructor(
    public k: number,
    public v: string,
    public n: number = 1,
    public d: Date = new Date(),
    public b: boolean = false
  ) { }
}

@Injectable()
export class DatepickerI18n extends NgbDatepickerI18n {

  constructor() {
    super();
  }

  I18N_VALUES = {
    weekdays: ['Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa', 'Do'],
    months: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
  };

  getWeekdayShortName(weekday: number): string {
    return this.I18N_VALUES.weekdays[weekday - 1];
  }
  getMonthShortName(month: number): string {
    return this.I18N_VALUES.months[month - 1];
  }
  getMonthFullName(month: number): string {
    return this.getMonthShortName(month);
  }

  getDayAriaLabel(date: NgbDateStruct): string {
    return `${date.day}-${date.month}-${date.year}`;
  }
}

@Injectable()
export class DateParserFormatter extends NgbDateParserFormatter {

  toInteger(value: any): number { return value; }
  padNumber(value: number): string { return ((value > 9) ? '' : '0') + value.toString(); }

  parse(value: string): NgbDateStruct {
    if (value) {
      const dateParts = value.trim().split('/');
      return { day: this.toInteger(dateParts[0]), month: this.toInteger(dateParts[1]), year: this.toInteger(dateParts[2]) };
    }
    return null;
  }

  format(date: NgbDateStruct): string {
    return date ?
      `${this.padNumber(date.day)}/${this.padNumber(date.month)}/${date.year}` :
      '';
  }
}

@Injectable()
export class DateAdapter extends NgbDateAdapter<Date> {

  fromModel(value: Date): NgbDateStruct {
    if (value) {
      return { day: value.getDate(), month: value.getMonth() + 1, year: value.getFullYear() };
    }
    return null;
  }

  toModel(date: NgbDateStruct): Date {
    if (date) {
      return new Date(date.year, date.month - 1, date.day);
    }
    return null
  }
}
