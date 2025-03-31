import { Component, inject, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { HideIfClaimsNotMetDirective } from '../../Core/directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../Core/utils/claimReq-utils';

@Component({
  selector: 'app-blank-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule, HideIfClaimsNotMetDirective],
  templateUrl: './blank-layout.component.html',
  styleUrl: './blank-layout.component.scss',
  animations: [
    trigger("toggleSideNavAnimation", [
      state("open", style({ left: "0" })),
      state("close", style({ left: "-16rem" })),
      transition("open <=> close", animate("1s ease-in-out"))
    ]),
    trigger("toggleNavAnimation", [
      state("open", style({ marginLeft: "20rem"})),
      state("close", style({ marginLeft: "4rem" })),
      transition("open <=> close", animate("1s ease-in-out"))
    ]),
  ]
})
export class BlankLayoutComponent {
  private readonly _router = inject(Router);
  private readonly _auth = inject(AuthService);
  claimReq = claimReq;
  
  onLogOut(){
    this._auth.deleteToken();
    this._router.navigateByUrl('/login');
  }

  userClaims : any;
  ngOnInit(){
    this.userClaims = this._auth.getClaims();
  }

  SideNavState : WritableSignal<string> = signal("close");
  toggleSideNav(){
    this.SideNavState.update((state) => state === "close" ? "open" : "close");
  }
}
