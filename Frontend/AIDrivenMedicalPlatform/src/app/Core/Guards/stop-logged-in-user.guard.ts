import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';

export const stopLoggedInUserGuard: CanActivateFn = (route, state) => {
  if(typeof localStorage !== 'undefined'){
    const _auth = inject(AuthService);
    if(!_auth.isLoggedIn()){
      return true;
    }
    else{
      const _router = inject(Router);
      const userClaims = _auth.getClaims();
      _router.navigateByUrl('/'+userClaims.role+"Dashboard");
    }
  }
  return false;
};
