import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CurrentLessonDto, LessonsServiceProxy, LessonStep, ProblemType, SubmitProblemAnswerInput } from '@shared/service-proxies/service-proxies';
import { NgWizardConfig, NgWizardService, StepChangedArgs, STEP_STATE, THEME } from 'ng-wizard';
import { Options } from '@angular-slider/ngx-slider';
import { AnswerResultModalComponent } from './answer-result-modal/answer-result-modal.component';

@Component({
    templateUrl: './lesson.component.html',
    styleUrls: ['./lesson.component.less'],
    animations: [appModuleAnimation()]
})
export class LessonComponent extends AppComponentBase implements OnInit {
    @ViewChild('answerResultModal') answerResultModal: AnswerResultModalComponent;
    StepStates = STEP_STATE;
    ProblemType = ProblemType;

    currentLesson: CurrentLessonDto = null;
    showNextLessonsOnScore = true;

    config: NgWizardConfig = {
        selected: 0,
        theme: THEME.default,
        keyNavigation: false,
        toolbarSettings: {
            showNextButton: false,
            showPreviousButton: false
            // toolbarExtraButtons: [{ text: 'Finish', class: 'btn btn-primary', event: () => { alert("Finished!!!"); } }]
        },
        anchorSettings: {
            anchorClickable: false,
            enableAllAnchors: false,
            markAllPreviousStepsAsDone: true,
            markDoneStep: false
        }
    };

    lessonFixedValue: number = 200;
    lessonSliderValue: number = 200;
    sliderOptions: Options = {
        floor: 100,
        ceil: 300,
        showTicks: true,
        tickStep: 50,
        readOnly: true,
        hideLimitLabels: true,
        showTicksValues: false
    };
    problemFreeAnswer: string = null;

    constructor(
        injector: Injector,
        private lessonsServiceProxy: LessonsServiceProxy,
        private ngWizardService: NgWizardService
    ) {
        super(injector);
    }

    get pageTitle() {
        if (this.currentLesson?.lesson != null) {
            return `Lesson "${this.currentLesson.lesson.name}"`;
        }

        return 'Lessons';
    }

    ngOnInit(): void {
        this.lessonsServiceProxy.getCurrentLesson().subscribe(x => {
            this.currentLesson = x;
            this.ngWizardService.show(this.getStepNumber(x.step));
        });
    }

    stepChanged(args: StepChangedArgs) {
        console.log(args.step);
    }

    toLesson() {
        this.ngWizardService.next();
    }

    toNext() {
        this.lessonsServiceProxy.moveToNextStep().subscribe(x => {
            this.currentLesson = x;
            if (x.step == LessonStep.MvpCompleted) {
                x.step = LessonStep.Score;
                this.showNextLessonsOnScore = false;
                this.message.success("Congratulations! You have successfully completed our subtraction course!", "Course completed");
            }
            this.ngWizardService.show(this.getStepNumber(x.step));
        });
        // this.ngWizardService.next();
    }

    goBack() {
        this.lessonsServiceProxy.moveToPreviousStep().subscribe(x => {
            this.currentLesson = x;
            this.ngWizardService.show(this.getStepNumber(x.step));
        });
    }

    submitProblem() {
        var answer = new SubmitProblemAnswerInput();
        answer.init({ problemId: this.currentLesson.problem.id });
        if (this.currentLesson.problem.type === ProblemType.FreeText) {
            answer.freeTextAnswer = this.problemFreeAnswer;
        }

        this.lessonsServiceProxy.submitProblemAnswer(answer).subscribe(x => {
            this.answerResultModal.show(
                x.isPreviousAnswerCorrect,
                this.currentLesson.problem.taskDescription,
                x.correctAnswer)
                .subscribe(_ => {
                    this.problemFreeAnswer = null;
                    this.currentLesson = x;
                    this.ngWizardService.show(this.getStepNumber(x.step));
                });
            // this.problemFreeAnswer = null;
            // this.currentLesson = x;
            // this.ngWizardService.show(this.getStepNumber(x.step));
        })
    }

    private getStepNumber(step: LessonStep) {
        switch (step) {
            case LessonStep.Score: return 0;
            case LessonStep.Lesson: return 1;
            case LessonStep.Activity: return 2;
            case LessonStep.ProblemSet: return 3;
            default: return 0;
        }
    }
}
