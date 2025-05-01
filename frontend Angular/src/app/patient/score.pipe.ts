import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'score',
  standalone: true
})
export class ScorePipe implements PipeTransform {
  transform(attributes: { score: number }[]): number {
    if (!attributes || attributes.length === 0) {
      return 0;
    }
    return attributes.reduce((sum, attr) => sum + attr.score, 0);
  }

}
