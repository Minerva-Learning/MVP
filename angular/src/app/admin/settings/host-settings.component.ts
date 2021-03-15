import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    ComboboxItemDto,
    CommonLookupServiceProxy,
    SettingScopes,
    HostSettingsEditDto,
    HostSettingsServiceProxy,
    SendTestEmailInput,
    JsonClaimMapDto
} from '@shared/service-proxies/service-proxies';
import { KeyValueListManagerComponent } from '@app/shared/common/key-value-list-manager/key-value-list-manager.component';
import { FormControl } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Component({
    templateUrl: './host-settings.component.html',
    styleUrls: ['./host-settings.component.less'],
    animations: [appModuleAnimation()]
})
export class HostSettingsComponent extends AppComponentBase implements OnInit {
    @ViewChild('wsFederationClaimsMappingManager') wsFederationClaimsMappingManager: KeyValueListManagerComponent;
    @ViewChild('openIdConnectClaimsMappingManager') openIdConnectClaimsMappingManager: KeyValueListManagerComponent;
    @ViewChild('emailSmtpSettingsForm') emailSmtpSettingsForm: FormControl;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;

    loading = false;
    hostSettings: HostSettingsEditDto;
    editions: ComboboxItemDto[] = undefined;
    testEmailAddress: string = undefined;
    showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
    defaultTimezoneScope: SettingScopes = SettingScopes.Application;

    usingDefaultTimeZone = false;
    initialTimeZone: string = undefined;

    enabledSocialLoginSettings: string[];

    wsFederationClaimMappings: { key: string, value: string }[];
    openIdConnectClaimMappings: { key: string, value: string }[];
    initialEmailSettings: string;
    uploadUrl: string;

    constructor(
        injector: Injector,
        private _hostSettingService: HostSettingsServiceProxy,
        private _httpClient: HttpClient,
        private _commonLookupService: CommonLookupServiceProxy
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + 'api/services/app/Lessons/ImportCouserFromExcel';
    }

    loadHostSettings(): void {
        const self = this;
        self._hostSettingService.getAllSettings()
            .subscribe(setting => {
                self.hostSettings = setting;
                self.initialTimeZone = setting.general.timezone;
                self.usingDefaultTimeZone = setting.general.timezoneForComparison === self.setting.get('Abp.Timing.TimeZone');

                this.wsFederationClaimMappings = this.hostSettings.externalLoginProviderSettings.openIdConnectClaimsMapping
                    .map(item => {
                        return {
                            key: item.key,
                            value: item.claim
                        };
                    });
                this.openIdConnectClaimMappings = this.hostSettings.externalLoginProviderSettings.openIdConnectClaimsMapping
                    .map(item => {
                        return {
                            key: item.key,
                            value: item.claim
                        };
                    });

                this.initialEmailSettings = JSON.stringify(self.hostSettings.email);
            });
    }

    loadEditions(): void {
        const self = this;
        self._commonLookupService.getEditionsForCombobox(false).subscribe((result) => {
            self.editions = result.items;

            const notAssignedEdition = new ComboboxItemDto();
            notAssignedEdition.value = null;
            notAssignedEdition.displayText = self.l('NotAssigned');

            self.editions.unshift(notAssignedEdition);
        });
    }

    init(): void {
        const self = this;
        self.testEmailAddress = self.appSession.user.emailAddress;
        self.showTimezoneSelection = abp.clock.provider.supportsMultipleTimezone;
        self.loadHostSettings();
        self.loadEditions();
        self.loadSocialLoginSettings();
    }

    ngOnInit(): void {
        const self = this;
        self.init();
    }

    sendTestEmail(): void {
        const self = this;
        const input = new SendTestEmailInput();
        input.emailAddress = self.testEmailAddress;

        if (this.initialEmailSettings !== JSON.stringify(this.hostSettings.email)) {
            this.message.confirm(
                this.l('SendEmailWithSavedSettingsWarning'),
                this.l('AreYouSure'),
                isConfirmed => {
                    if (isConfirmed) {
                        self._hostSettingService.sendTestEmail(input).subscribe(result => {
                            self.notify.info(self.l('TestEmailSentSuccessfully'));
                        });
                    }
                }
            );
        } else {
            self._hostSettingService.sendTestEmail(input).subscribe(result => {
                self.notify.info(self.l('TestEmailSentSuccessfully'));
            });
        }
    }

    mapClaims(): void {
        if (this.wsFederationClaimsMappingManager) {
            this.hostSettings.externalLoginProviderSettings.wsFederationClaimsMapping = this.wsFederationClaimsMappingManager.getItems()
                .map(item =>
                    new JsonClaimMapDto({
                        key: item.key,
                        claim: item.value
                    })
                );
        }

        if (this.openIdConnectClaimsMappingManager) {
            this.hostSettings.externalLoginProviderSettings.openIdConnectClaimsMapping = this.openIdConnectClaimsMappingManager.getItems()
                .map(item =>
                    new JsonClaimMapDto({
                        key: item.key,
                        claim: item.value
                    })
                );
        }
    }

    saveAll(): void {
        if (!this.isSmtpSettingsFormValid()) {
            return;
        }

        const self = this;

        self.mapClaims();
        if (!self.hostSettings.tenantManagement.defaultEditionId || self.hostSettings.tenantManagement.defaultEditionId.toString() === 'null') {
            self.hostSettings.tenantManagement.defaultEditionId = null;
        }

        self._hostSettingService.updateAllSettings(self.hostSettings).subscribe(result => {
            self.notify.info(self.l('SavedSuccessfully'));

            if (abp.clock.provider.supportsMultipleTimezone && self.usingDefaultTimeZone && self.initialTimeZone !== self.hostSettings.general.timezone) {
                self.message.info(self.l('TimeZoneSettingChangedRefreshPageNotification')).then(() => {
                    window.location.reload();
                });
            }

            this.initialEmailSettings = JSON.stringify(self.hostSettings.email);
        });
    }

    loadSocialLoginSettings(): void {
        const self = this;
        this._hostSettingService.getEnabledSocialLoginSettings()
            .subscribe(setting => {
                self.enabledSocialLoginSettings = setting.enabledSocialLoginSettings;
            });
    }

    isSocialLoginEnabled(name: string): boolean {
        return this.enabledSocialLoginSettings &&
            this.enabledSocialLoginSettings.indexOf(name) !== -1;
    }

    isSmtpSettingsFormValid(): boolean {
        return this.emailSmtpSettingsForm.valid;
    }

    uploadExcel(data: { files: File }): void {
        const formData: FormData = new FormData();
        const file = data.files[0];
        formData.append('file', file, file.name);

        this._httpClient
            .post<any>(this.uploadUrl, formData)
            .pipe(finalize(() => this.excelFileUpload.clear()))
            .subscribe(response => {
                if (response.success) {
                    this.notify.success(this.l('Course was uploaded.'));
                } else if (response.error != null) {
                    this.notify.error(this.l('Error occured during importing course.'));
                }
            });
    }

    onUploadExcelError(): void {
        this.notify.error(this.l('ImportUsersUploadFailed'));
    }
}
