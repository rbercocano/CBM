import { Pipe, PipeTransform, } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';

@Pipe({
  name: 'cbmCurrency'
})
export class CbmCurrencyPipe implements PipeTransform {
  /**
   *
   */
  constructor(private decimalPipe: DecimalPipe, private authService: AuthService) {

  }
  transform(value: number, precision: number = 2): string {
    let currency = this.authService.userData.currency ?? 'R$';
    precision = precision ?? 2;
    let vDecimal = this.decimalPipe.transform(value, `1.2-${precision}`);
    return `${currency} ` + vDecimal.toString()
      .replace(/,/g, "x")
      .replace(/\./g, ",")
      .replace(/x/g, ".");
  }

}
