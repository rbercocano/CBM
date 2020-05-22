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
  transform(value: number): string {
    var vDecimal = this.decimalPipe.transform(value, '1.2-2');
    return 'R$ ' + vDecimal.toString()
                      .replace(/,/g, "x")
                      .replace(/\./g, ",")
                      .replace(/x/g, ".");
  }

}
