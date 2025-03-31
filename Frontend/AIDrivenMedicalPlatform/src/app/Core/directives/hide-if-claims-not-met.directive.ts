import { AuthService } from './../Services/auth.service';
import { Directive, ElementRef, Input, OnInit } from '@angular/core';

@Directive({
  selector: '[appHideIfClaimsNotMet]'
})
export class HideIfClaimsNotMetDirective implements OnInit{
  @Input("appHideIfClaimsNotMet") claimReq ! : Function;

  constructor(private _auth : AuthService, private elementRef : ElementRef) { }

  ngOnInit(): void {
    const claims = this._auth.getClaims();
    if(!this.claimReq(claims)){
      this.elementRef.nativeElement.style.display = "none";
    }
  }

}
