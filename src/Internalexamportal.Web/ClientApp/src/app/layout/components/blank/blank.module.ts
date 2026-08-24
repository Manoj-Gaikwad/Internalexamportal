import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { FuseSharedModule } from '@fuse/shared.module';
import { FuseSidebarModule } from '@fuse/components';
import { BlankComponent } from 'app/layout/components/blank/blank.component';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
     MatButtonModule,
     MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    FuseSharedModule,
    FuseSidebarModule

  ]
})
export class BlankModule { }
