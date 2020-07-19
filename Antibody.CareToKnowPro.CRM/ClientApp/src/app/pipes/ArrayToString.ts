import { PipeTransform, Pipe } from "@angular/core";

@Pipe({ name: 'ArrayToString' })
export class ArrayToStringPipe implements PipeTransform {
    constructor() { }
    transform(value: Array<any>, separator = ',') {
        if (value) {
             var string = value.map(x=>x).join(separator)
            return string;
        }
        return '';
    }
}
