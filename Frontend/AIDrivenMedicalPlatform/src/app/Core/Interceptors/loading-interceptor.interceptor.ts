import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs';

export const loadingInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const _NgxSpinnerService = inject(NgxSpinnerService);

  _NgxSpinnerService.show(undefined, {
    type: 'square-jelly-box',
    size: 'large',
    color: '#28a745',
    bdColor: 'rgba(0, 0, 0, 0.5)',
    fullScreen: true,
    template: '<div class="spinner-container"><div class="spinner"></div><span class="spinner-text">Please wait...</span></div>',
  });

  return next(req).pipe(finalize(()=>{
    _NgxSpinnerService.hide();
  }));
};
