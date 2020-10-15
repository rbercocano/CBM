import { Pipe, PipeTransform } from '@angular/core';
import { DecimalPipe } from '@angular/common';

@Pipe({
  name: 'cbmNumber'
})
export class CbmNumberPipe implements PipeTransform {

  constructor(private decimalPipe: DecimalPipe) {

  }
  transform(value: number): string {
    if(value == null) return '';
    var vDecimal = this.decimalPipe.transform(value, '1.2-2');
    return vDecimal.toString()
                      .replace(/,/g, "x")
                      .replace(/\./g, ",")
                      .replace(/x/g, ".");
  }

}
