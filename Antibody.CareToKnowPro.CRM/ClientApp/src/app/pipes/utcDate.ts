import * as moment from 'moment';
import { PipeTransform, Pipe } from "@angular/core";

@Pipe({ name: 'UTCToLocalDate' })
export class UTCToLocalDatePipe implements PipeTransform {
  constructor() { }
  transform(value) {
    if (value) {
      return moment.utc(value).local().format("LLL");
    } else {
      return '';
    }
  }
}
