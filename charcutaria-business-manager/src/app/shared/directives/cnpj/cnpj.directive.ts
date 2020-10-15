import { Directive } from '@angular/core';
import { Validator, ValidatorFn, FormControl, AbstractControl, NG_VALIDATORS } from '@angular/forms';
import { ValidatorService } from '../../services/validator/validator.service';

@Directive({
  selector: '[cnpj][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: CnpjDirective, multi: true }
  ]
})
export class CnpjDirective implements Validator {
  validator: ValidatorFn;

  constructor(private validatorService: ValidatorService) {
    this.validator = cnpjValidatorFactory(this.validatorService);
  }

  validate(c: FormControl) {
    return this.validator(c);
  }

}
function cnpjValidatorFactory(validatorService: ValidatorService): ValidatorFn {
  return (c: AbstractControl) => {

    let isValid = validatorService.isValidCNPJ(c.value);

    if (isValid) {
      return null;
    } else {
      return {
        cnpj: {
          valid: false
        }
      };
    }
  }
}
