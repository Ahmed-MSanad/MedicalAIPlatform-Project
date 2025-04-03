import { Pipe, PipeTransform } from '@angular/core';
import { Gender } from '../../Enums/gender';

@Pipe({
  name: 'gender'
})
export class GenderPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return value === 0 ? 'Male' : 'Female';
  }

}
