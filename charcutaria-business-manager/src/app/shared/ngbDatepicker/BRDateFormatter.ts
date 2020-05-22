import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Injectable } from '@angular/core';

@Injectable()
export class BRDateFormatter extends NgbDateParserFormatter {
    parse(value: string): NgbDateStruct {
        if (value) {
            const dateParts = value.trim().split('/');
            return { day: Number(dateParts[0]), month: Number(dateParts[1]), year: Number(dateParts[2]) };
        }
        return null;
    }
    format(date: NgbDateStruct): string {
        return date ? `${date.day}/${date.month}/${date.year}` : '';
    }
}