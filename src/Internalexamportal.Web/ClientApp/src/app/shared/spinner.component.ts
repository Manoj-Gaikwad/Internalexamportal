import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'ep-spinner',
    template: `
        @if (isSpinner) {
          <div class="loading-indicator" >
            <mat-progress-spinner
              class="example-margin"
              color="primary"
              mode="indeterminate">
            </mat-progress-spinner>
          </div>
        }
        `,
    styles: [`
        /* Absolute Center Spinner */
        .loading-indicator {
            position: fixed;
            z-index: 999;
            height: 2em;
            width: 2em;
            overflow: show;
            margin: auto;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
        }
        
        /* Transparent Overlay */
        .loading-indicator:before {
            content: '';
            display: block;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.3);
        }
    `],
    standalone: false
})
  export class SpinnerComponent implements OnInit{

    @Input() isSpinner: boolean;
    
    ngOnInit() {

    }
  }