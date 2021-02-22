import { memoize } from 'lodash';
import { DateTime, Info, Zone } from 'luxon';

export function toUtcDate(date: Date): DateTime {
    let year = date.getUTCFullYear();
    let month = date.getMonth() + 1;
    let day = date.getDate();
    // let str = `${year}-${month}-${day}`;
    return DateTime.utc(year, month, day);
}

// export const getTimezonesList = memoize(() => {
//     const names = moment.tz.names();
//     const zones = names.map(x => moment.tz.zone(x));
//     return zones;
// });

export function userTzNow() {
    let now = DateTime.fromJSDate(new Date(abp.clock.now()));
    if (isCurrentProviderUtc()) {
        now = now.toUTC();
    }

    return now;
}

export function isCurrentProviderUtc() {
    return abp.clock.provider === abp.timing.utcClockProvider;
}

export function getTimeInUserTz(date: DateTime | Date) {
    let dt = date instanceof Date ? date : date.toJSDate();
    return DateTime.fromJSDate(abp.timing.convertToUserTimezone(dt));
}
