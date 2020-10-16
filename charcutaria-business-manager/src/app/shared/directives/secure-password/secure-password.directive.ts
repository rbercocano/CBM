import { Directive } from '@angular/core';
import { NG_VALIDATORS, Validator, ValidatorFn, FormControl, AbstractControl } from '@angular/forms';
import { ValidatorService } from '../../services/validator/validator.service';

@Directive({
  selector: '[securePassword][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: SecurePasswordDirective, multi: true }
  ]
})
export class SecurePasswordDirective implements Validator {
  validator: ValidatorFn;

  constructor(private validatorService: ValidatorService) {
    this.validator = pwdValidatorFactory(this.validatorService);
  }

  validate(c: FormControl) {
    return this.validator(c);
  }

}
function pwdValidatorFactory(validatorService: ValidatorService): ValidatorFn {
  return (c: AbstractControl) => {

    let isValid = validatorService.isPasswordSecure(c.value);

    if (isValid) {
      return null;
    } else {
      return {
        securePassword: {
          valid: false
        }
      };
    }
  }
}
