import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'cg-content-header',
    templateUrl: './content-header.component.html',
    styleUrls: ['./content-header.component.scss'],
    standalone: false
})
export class ContentHeaderComponent implements OnInit {

  @Input() header: string;
  constructor() { }

  ngOnInit() {
  }

}
