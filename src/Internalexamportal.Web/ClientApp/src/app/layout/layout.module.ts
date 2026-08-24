import { NgModule } from '@angular/core';

import { VerticalLayout1Module } from 'app/layout/vertical/layout-1/layout-1.module';

import { HorizontalLayout1Module } from 'app/layout/horizontal/layout-1/layout-1.module';
import { VerticalLayout3Module } from './vertical/layout-3/layout-3.module';

@NgModule({
    imports: [
        VerticalLayout1Module,
        HorizontalLayout1Module,
        VerticalLayout3Module
    ],
    exports: [
        VerticalLayout1Module,

        HorizontalLayout1Module,

        VerticalLayout3Module
    ],
    declarations: []
})
export class LayoutModule
{
}
