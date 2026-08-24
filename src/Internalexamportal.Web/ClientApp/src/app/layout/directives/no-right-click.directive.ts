import { Directive, HostListener } from '@angular/core';

@Directive({
    selector: '[appNoRightClick]',
    standalone: false
})
export class NoRightClickDirective {

    @HostListener('contextmenu', ['$event'])
    onRightClick(event) {
        event.preventDefault();
    }

    constructor() { }

}