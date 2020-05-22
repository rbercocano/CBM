import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable()
export class BRDateAdapter extends NgbDateAdapter<string> {

    readonly DELIMITER = '/';

    fromModel(value: string | null): NgbDateStruct | null {
        if (value) {
            let date = new Date(value);
            let day = date.getUTCDate();
            return {
                day: day,
                month: date.getMonth() + 1,
                year: date.getFullYear()
            };
        }
        return null;
    }

    toModel(date: NgbDateStruct | null): string | null {
        return date ? new Date(date.year, date.month, date.day).toISOString() : null;
    }
}