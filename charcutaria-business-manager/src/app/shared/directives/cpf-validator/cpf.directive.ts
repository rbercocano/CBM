import { Directive } from '@angular/core';
import { Validator, ValidatorFn, FormControl, AbstractControl, NG_VALIDATORS } from '@angular/forms';
import { ValidatorService } from '../../services/validator/validator.service';

@Directive({
  selector: '[cpf][ngModel]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: CpfDirective, multi: true }
  ]
})
export class CpfDirective implements Validator {
  validator: ValidatorFn;

  constructor(private validatorService: ValidatorService) {
    this.validator = cpfValidatorFactory(this.validatorService);
  }

  validate(c: FormControl) {
    return this.validator(c);
  }

}
function cpfValidatorFactory(validatorService: ValidatorService): ValidatorFn {
  return (c: AbstractControl) => {

    let isValid = validatorService.isValidCPF(c.value);

    if (isValid) {
      return null;
    } else {
      return {
        cpf: {
          valid: false
        }
      };
    }
  }
}
