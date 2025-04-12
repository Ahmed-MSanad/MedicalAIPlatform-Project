import { NgxSpinnerConfig } from './../../node_modules/ngx-spinner/lib/config.d';
import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';

import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideToastr } from 'ngx-toastr';
import { requestHeaderHandlingInterceptor } from './Core/Interceptors/request-header-handling.interceptor';

import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideRouter(routes),

    provideClientHydration(withEventReplay()),

    provideAnimationsAsync(),

    provideToastr({positionClass: 'toast-top-center'}),

    provideHttpClient(withFetch(), withInterceptors([requestHeaderHandlingInterceptor])),    

    importProvidersFrom(
      NgxSpinnerModule,
    ),
    // {
    //   provide: NgxSpinnerService,
    //   useFactory: () => {
    //     const spinner = new NgxSpinnerService();
        
    //     spinner.show(undefined, {
    //       type: 'square-jelly-box',
    //       size: 'large',
    //       color: '#28a745',
    //       bdColor: 'rgba(0, 0, 0, 0.5)',
    //       fullScreen: true,
    //       template: '<div class="spinner-container"><div class="spinner"></div><span class="spinner-text">Please wait...</span></div>',
    //     });
    //     return spinner;
    //   },
    // },


  ]
};
