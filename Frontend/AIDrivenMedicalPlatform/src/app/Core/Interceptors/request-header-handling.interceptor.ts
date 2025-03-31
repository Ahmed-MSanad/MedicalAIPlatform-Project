import { HttpHeaders, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../Services/auth.service';
import { tap } from 'rxjs';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const requestHeaderHandlingInterceptor: HttpInterceptorFn = (req, next) => {
  const _auth = inject(AuthService);
  const _router = inject(Router);
  const _toastr = inject(ToastrService);

  if(_auth.isValidLocalStorage()){
    if(_auth.isLoggedIn()){
      const clonedReq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + _auth.getToken())
      });
      return next(clonedReq).pipe(
        tap({
          error: (err:any) => {
            if(err.status == 401){ // Token isn't valid
              _auth.deleteToken();
              setTimeout(() => {
                _toastr.info("Login again", "Session Expired!");
              }, 1000);
              _router.navigate(['/login']);
            }
            else if(err.status == 403){
              _toastr.error("Sorry, You're not authorized to perform that action!!");
            }
          }
        })
      );
    }
  }
  return next(req);
};
