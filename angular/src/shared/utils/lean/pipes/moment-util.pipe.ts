import { PipeTransform, Pipe } from '@angular/core';
import { getTimeInUserTz } from '@shared/utils/lean/time-utils';
import { DateTime } from 'luxon';

@Pipe({ name: 'userTzMoment' })
export class UserTimeZoneMoment implements PipeTransform {
    transform(value: DateTime | Date) {
        return getTimeInUserTz(value);
    }
}

@Pipe({ name: 'mToNow' })
export class MomentToNow implements PipeTransform {
    transform(value: DateTime) {
        return value.toRelative({
            locale: abp.localization.currentLanguage.name,
        });
    }
}

@Pipe({ name: 'mFromNow' })
export class MomentFromNow implements PipeTransform {
    transform(value: DateTime) {
        return value.toRelative({
            locale: abp.localization.currentLanguage.name,
        });
    }
}
