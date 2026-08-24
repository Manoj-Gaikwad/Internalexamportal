import { Component } from '@angular/core';

@Component({
    selector: 'footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    standalone: false
})
export class FooterComponent {
    public date: Date;
    /**
     * Constructor
     */
    constructor() {
        this.date = new Date();
    }
}
