import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  if(typeof localStorage !== 'undefined'){
    const _auth = inject(AuthService);
    const _router = inject(Router);
    if(_auth.isLoggedIn()){
      const claimReq = route.data['claimReq'] as Function;
      if(claimReq){ // Check if there is claimReq or not for this route path
        const claims = _auth.getClaims();
        if(!claimReq(claims)){ // if the logged in user claims is not as what we expect the return false
          _router.navigate(['/forbidden']);
          return false;
        }
        return true;
      }
      return true;
    }
    else{ // you're not authenticated to this routing
      const _router = inject(Router);
      _router.navigateByUrl('/login');
    }
  }
  return false;
};
