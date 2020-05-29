import { Pipe, PipeTransform, } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'cbmCurrency'
})
export class CbmCurrencyPipe implements PipeTransform {
  /**
   *
   */
  constructor(private decimalPipe: DecimalPipe) {

  }
  transform(value: number, precision: number = 2): string {
    precision = precision ?? 2;
    var vDecimal = this.decimalPipe.transform(value, `1.2-${precision}`);
    return 'R$ ' + vDecimal.toString()
      .replace(/,/g, "x")
      .replace(/\./g, ",")
      .replace(/x/g, ".");
  }

}
