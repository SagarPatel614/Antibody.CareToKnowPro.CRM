import { PipeTransform, Pipe } from "@angular/core";

@Pipe({ name: 'SplitOnUppercase' })
export class SplitOnUppercasePipe implements PipeTransform {
    constructor() { }
    transform(value: string, separator = ' ') {
        if (value) {
            return value.split(/(?=[A-Z])/).join(separator);
        }
        return '';
    }
}
