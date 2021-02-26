import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LessonsServiceProxy } from '@shared/service-proxies/service-proxies';
import { NgWizardConfig, StepChangedArgs, STEP_STATE, THEME } from 'ng-wizard';

@Component({
    templateUrl: './lesson.component.html',
    styleUrls: ['./lesson.component.less'],
    animations: [appModuleAnimation()]
})
export class LessonComponent extends AppComponentBase implements OnInit {
    stepStates = STEP_STATE;

    config: NgWizardConfig = {
        selected: 0,
        theme: THEME.default,
        toolbarSettings: {
            toolbarExtraButtons: [{ text: 'Finish', class: 'btn btn-primary', event: () => { alert("Finished!!!"); } }]
        }
    };

    constructor(
        injector: Injector,
        private lessonsServiceProxy: LessonsServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.lessonsServiceProxy.getCurrentLesson().subscribe(x => {

        });
    }

    stepChanged(args: StepChangedArgs) {
        console.log(args.step);
    }
}
