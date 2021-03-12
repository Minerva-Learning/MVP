import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';

const correctMessages = [
    'You figured it out!',
    'You know how to think mathematically!',
    'Way to subtract!',
    'Your effort is awesome!',
    'You\'re facing this challenge like a champ!'
];
const incorrectMessages = [
    'Almost there!  The correct answer is {0}. Try a new question.',
    'Try again! Practice helps you learn. The correct answer is {0}.',
    'Good try! The correct answer is {0}. Keep going!',
    'Keep thinking!  The correct answer is {0}.',
    'Learning math takes time! The correct answer is {0}. Let\'s do another problem.'
];
function getRandomInt(max: number) {
    return Math.floor(Math.random() * Math.floor(max + 1));
  }

@Component({
    selector: 'app-answer-result-modal',
    templateUrl: './answer-result-modal.component.html',
    styleUrls: ['./answer-result-modal.component.less']
})
export class AnswerResultModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('answerResultModal', { static: true }) modal: ModalDirective;
    saving = false;
    private currentCloseSybject: ReplaySubject<void> = null;
    isAnswerCorrect = false;
    problemText: string = null;
    messageText: string = null;

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
    }

    close(): void {
        this.currentCloseSybject.next();
        this.disposeSubject();
        this.messageText = null;
        this.problemText = null;
        this.modal.hide();
    }

    show(isAnswerCorrect: boolean, problemText: string, correctAnswer?: string) {
        this.disposeSubject();
        this.currentCloseSybject = new ReplaySubject<void>(1);
        this.setMessage(isAnswerCorrect, correctAnswer);
        if (isAnswerCorrect) {
            this.notify.success(this.messageText, null, { position: 'top', timer: 2000, customClass: { popup: 'success-toast' } });
            this.currentCloseSybject.next();
        }
        else {
            this.isAnswerCorrect = isAnswerCorrect;
            this.problemText = problemText;
            this.modal.show();
        }

        return this.currentCloseSybject.asObservable();
    }

    private setMessage(isAnswerCorrect: boolean, correctAnswer?: string) {
        this.messageText = isAnswerCorrect
            ? correctMessages[getRandomInt(correctMessages.length - 1)]
            : abp.utils.formatString(incorrectMessages[getRandomInt(incorrectMessages.length - 1)], correctAnswer);
    }

    private disposeSubject() {
        this.currentCloseSybject?.complete();
        this.currentCloseSybject?.unsubscribe();
        this.currentCloseSybject = null;
    }
}
