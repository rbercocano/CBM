import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { NG_VALIDATORS, Validator, ValidatorFn, FormControl, AbstractControl } from '@angular/forms';
import { ValidatorService } from '../../services/validator/validator.service';

@Directive({
  selector: '[mobilePhone]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: MobilePhoneDirective, multi: true }
  ]
})
export class MobilePhoneDirective implements Validator, OnInit {
  validator: ValidatorFn;
  private country:string;
  // @Input() country: string;
  constructor(private validatorService: ValidatorService, private el: ElementRef) {
    this.country = this.el.nativeElement.attributes["country"].value;
    this.validator = phoneValidatorFactory(this.validatorService, this.country);
  }
  ngOnInit(): void {
  }

  validate(c: FormControl) {
    return this.validator(c);
  }

}
function phoneValidatorFactory(validatorService: ValidatorService, country: string): ValidatorFn {
  return (c: AbstractControl) => {

    let isValid = validatorService.isValidMobile(c.value, country);

    if (isValid) {
      return null;
    } else {
      return {
        mobile: {
          valid: false
        }
      };
    }
  }
}
