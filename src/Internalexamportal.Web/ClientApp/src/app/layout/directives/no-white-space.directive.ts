import { Directive, HostListener } from '@angular/core';

@Directive({
    selector: '[NoWhiteSpace]',
    standalone: false
})
export class NoWhiteSpaceDirective {

  constructor() { }

  @HostListener('document:keydown.space', ['$event']) onKeydownHandler(event: KeyboardEvent) {
    if (event.srcElement['selectionStart'] == 0) {
      event.preventDefault();
    }
  }
}
