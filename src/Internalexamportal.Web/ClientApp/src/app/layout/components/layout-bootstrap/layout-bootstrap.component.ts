import { Component, OnInit } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { FuseSidebarService } from '@fuse/components/sidebar/sidebar.service';

@Component({
    selector: 'cg-layout-bootstrap',
    templateUrl: './layout-bootstrap.component.html',
    styleUrls: ['./layout-bootstrap.component.scss'],
    standalone: false
})
export class LayoutBootstrapComponent implements OnInit {
  _unsubscribeAll: Subject<any>;
  fuseConfig: any;

  constructor(
    private _fuseConfigService: FuseConfigService,
    private _fuseSidebarService: FuseSidebarService) {
    this._unsubscribeAll = new Subject();
  }

  ngOnInit() {
    this._fuseConfigService.config
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((config) => {

        this.fuseConfig = config;
      });
  }

  toggleSidebarOpen(key): void {
    this._fuseSidebarService.getSidebar(key).toggleOpen();
  }
}
