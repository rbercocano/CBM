import { Directive, Input, OnInit } from '@angular/core';
import { Validator, ValidatorFn, FormControl, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
  selector: '[minValue]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: MinValueDirective, multi: true }
  ]
})
export class MinValueDirective implements Validator, OnInit {
  validator: ValidatorFn;
  @Input() minValue: number;
  constructor() { }
  ngOnInit(): void {
    this.validator = minValueValitadorFactory(this.minValue);
  }

  validate(c: FormControl) {
    return this.validator(c);
  }

}
function minValueValitadorFactory(minValue: number): ValidatorFn {
  return (c: AbstractControl) => {

    let isValid = c.value >= minValue;

    if (isValid) {
      return null;
    } else {
      return {
        minValue: {
          valid: false
        }
      };
    }
  }
}
