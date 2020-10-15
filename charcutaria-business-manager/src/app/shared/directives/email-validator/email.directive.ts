import { Directive } from '@angular/core';
import { AbstractControl, FormControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[email][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: EmailDirective, multi: true }
  ]
})
export class EmailDirective implements Validator  {
  validator: ValidatorFn;

  constructor() {
    this.validator = emailValidatorFactory();
  }
  
  validate(c: FormControl) {
    return this.validator(c);
  }

}
function emailValidatorFactory(): ValidatorFn {
    return (c: AbstractControl) => {

        let regexp = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
        let isValid = regexp.test(c.value);

        if (isValid) {
            return null;
        } else {
            return {
                email: {
                    valid: false
                }
            };
        }
    }
}
