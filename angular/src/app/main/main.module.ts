import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { UtilsModule } from '@shared/utils/utils.module';
import { CountoModule } from 'angular2-counto';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MainRoutingModule } from './main-routing.module';
import { NgWizardModule, NgWizardConfig, THEME } from 'ng-wizard';

import { BsDatepickerConfig, BsDaterangepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { LessonComponent } from './lesson/lesson.component';
import { StepsModule } from 'primeng/steps';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { AnswerResultModalComponent } from './lesson/answer-result-modal/answer-result-modal.component';
import { AppBsModalModule } from '@shared/common/appBsModal/app-bs-modal.module';

NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales();

const ngWizardConfig: NgWizardConfig = { theme: THEME.default };

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        TabsModule,
        TooltipModule,
        AppCommonModule,
        UtilsModule,
        MainRoutingModule,
        CountoModule,
        BsDatepickerModule.forRoot(),
        BsDropdownModule.forRoot(),
        PopoverModule.forRoot(),
        NgWizardModule.forRoot(ngWizardConfig),
        ModalModule,
        AppBsModalModule,
        StepsModule,
        NgxSliderModule
    ],
    declarations: [
        DashboardComponent,
        LessonComponent,
        AnswerResultModalComponent
    ],
    providers: [
        { provide: BsDatepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerConfig },
        { provide: BsDaterangepickerConfig, useFactory: NgxBootstrapDatePickerConfigService.getDaterangepickerConfig },
        { provide: BsLocaleService, useFactory: NgxBootstrapDatePickerConfigService.getDatepickerLocale }
    ]
})
export class MainModule { }
