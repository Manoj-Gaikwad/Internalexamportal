import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// Angular Material modules (must be imported individually)
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatRadioModule } from '@angular/material/radio';
import { MatStepperModule } from '@angular/material/stepper';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatRippleModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatMenuModule } from '@angular/material/menu';

// Moment date adapter
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { MAT_DATE_FORMATS } from '@angular/material/core';

// Forms & routing
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

// Third-party & shared
import { FuseSharedModule } from '@fuse/shared.module';
import { Error404Module } from 'app/main/errors/404/error-404.module';
import { FuseCountdownModule } from '@fuse/components';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { WebcamModule } from 'ngx-webcam';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

// Custom directives & components
import { NoWhiteSpaceDirective } from 'app/layout/directives/no-white-space.directive';
import { ContentHeaderComponent } from 'app/layout/components/content-header/content-header.component';
import { SpinnerComponent } from './spinner.component';
import { PermissionBasedAccessDirective } from 'app/layout/directives/permissionBasedAccess.directive';
import { WebCamDialogComponent } from 'app/layout/dialogs/webcam/webcamdialog.component';
import { BlockCopyPasteDirective } from 'app/layout/directives/blockCopyPaste.directive';
import { DATE_FORMAT } from 'app/layout/entities/globalConstants';
import { QuillModule } from 'ngx-quill';
import { FSEditorComponent } from './fs-editor-component/fs-editor-component.component';

@NgModule({
    declarations: [
        NoWhiteSpaceDirective,
        ContentHeaderComponent,
        SpinnerComponent,
        PermissionBasedAccessDirective,
        WebCamDialogComponent,
        BlockCopyPasteDirective,
        FSEditorComponent
    ],
    imports: [
        CommonModule,
        FuseSharedModule,
        Error404Module,
        ReactiveFormsModule,
        FormsModule,
        RouterModule,

        // Angular Material
        MatIconModule,
        MatButtonModule,
        MatFormFieldModule,
        MatCheckboxModule,
        MatInputModule,
        MatDialogModule,
        MatProgressSpinnerModule,
        MatTabsModule,
        MatOptionModule,
        MatSelectModule,
        MatGridListModule,
        MatDividerModule,
        MatListModule,
        MatRadioModule,
        MatStepperModule,
        MatAutocompleteModule,
        MatRippleModule,
        MatToolbarModule,
        MatSidenavModule,
        MatExpansionModule,
        MatDatepickerModule,
        MatCardModule,
        MatTooltipModule,
        MatPaginatorModule,
        MatSortModule,
        MatTableModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatMenuModule,
        MatMomentDateModule,

        // Third-party
        NgxChartsModule,
        QuillModule.forRoot(),
        NgxMaterialTimepickerModule,
        WebcamModule,
        NgxMatSelectSearchModule,
        FuseCountdownModule
    ],
    exports: [
        FuseSharedModule,
        Error404Module,
        ReactiveFormsModule,
        FormsModule,
        RouterModule,

        // Angular Material (re-export)
        MatIconModule,
        MatButtonModule,
        MatFormFieldModule,
        MatCheckboxModule,
        MatInputModule,
        MatDialogModule,
        MatProgressSpinnerModule,
        MatTabsModule,
        MatOptionModule,
        MatSelectModule,
        MatGridListModule,
        MatDividerModule,
        MatListModule,
        MatRadioModule,
        MatStepperModule,
        MatAutocompleteModule,
        MatRippleModule,
        MatToolbarModule,
        MatSidenavModule,
        MatExpansionModule,
        MatDatepickerModule,
        MatCardModule,
        MatTooltipModule,
        MatPaginatorModule,
        MatSortModule,
        MatTableModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatMenuModule,
        MatMomentDateModule,

        // Third-party
        NgxChartsModule,
        QuillModule,
        NgxMaterialTimepickerModule,
        WebcamModule,
        NgxMatSelectSearchModule,
        FuseCountdownModule,

        // Custom
        NoWhiteSpaceDirective,
        ContentHeaderComponent,
        SpinnerComponent,
        PermissionBasedAccessDirective,
        WebCamDialogComponent,
        BlockCopyPasteDirective,
        FSEditorComponent
    ],
    providers: [
        {
            provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS,
            useValue: { useUtc: true }
        },
        { provide: MAT_DATE_FORMATS, useValue: DATE_FORMAT }
    ]
})
export class SharedModule {}
