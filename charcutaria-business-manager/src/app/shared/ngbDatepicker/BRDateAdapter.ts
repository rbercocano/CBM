import { Injectable } from '@angular/core';
import { NgbDateAdapter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';
@Injectable()
export class BRDateAdapter extends NgbDateAdapter<string> {

    readonly DELIMITER = '/';

    fromModel(value: string | null): NgbDateStruct | null {
        if (value) {
            let date = moment(value);
            let d = {
                day: parseInt(date.format("DD")),
                month: parseInt(date.format("MM")),
                year: parseInt(date.format("YYYY"))
            };
            return d;
        }
        return null;
    }

    toModel(date: NgbDateStruct | null): string | null {
        return date ?
            moment(`${date.year}-${date.month}-${date.day}`).format() : null;
    }
}