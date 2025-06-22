import { Component, inject, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../Core/Services/auth.service';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { HideIfClaimsNotMetDirective } from '../../Core/directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../Core/utils/claimReq-utils';
import { TranslationService } from '../../Core/Services/translation.service';
import { TranslateModule, TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-blank-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule, HideIfClaimsNotMetDirective, TranslateModule],
  templateUrl: './blank-layout.component.html',
  styleUrl: './blank-layout.component.scss',
  animations: [
    trigger("toggleSideNavAnimation", [
      state("open-en", style({ left: "0", right: "auto" })),
      state("close-en", style({ left: "-16rem", right: "auto" })),
      state("open-ar", style({ right: "0", left: "auto" })),
      state("close-ar", style({ right: "-16rem", left: "auto" })),
      transition("* <=> *", animate("0.5s ease-in-out"))
    ]),
    trigger("toggleNavAnimation", [
      state("open-en", style({ marginLeft: "20rem", marginRight: "0" })),
      state("close-en", style({ marginLeft: "4rem", marginRight: "0" })),
      state("open-ar", style({ marginRight: "20rem", marginLeft: "0" })),
      state("close-ar", style({ marginRight: "4rem", marginLeft: "0" })),
      transition("* <=> *", animate("0.5s ease-in-out"))
    ])
  ]

})
export class BlankLayoutComponent {
  private readonly _router = inject(Router);
  private readonly _auth = inject(AuthService);
  private readonly _translate = inject(TranslationService);
  claimReq = claimReq;
  currentLanguage = 'en';

  get dashboard(): string {
    return `navbar.${this.userClaims.role} Dashboard`;
  }

  get service(): string {
    return `navbar.${this.userClaims.role} Services`;
  }

  get profile(): string {
    return `navbar.${this.userClaims.role} Profile`;
  }

  onLogOut() {
    this._auth.deleteToken();
    this._router.navigateByUrl('/login');
  }

  currentWindowWidth: WritableSignal<number> = signal(0);
  userClaims: any;
  ngOnInit() {
    this.userClaims = this._auth.getClaims();
    if (typeof (window) !== 'undefined') {
      window.addEventListener('resize', this.updateWindowWidth);
    }
    if (localStorage.getItem('lang') == 'ar') {
      this.currentLanguage = 'ar';
    }
  }

  updateWindowWidth = () => {
    this.currentWindowWidth.set(window.innerWidth);
  };

  SideNavState: WritableSignal<string> = signal("close");
  toggleSideNav() {
    this.SideNavState.update((state) => state === "close" ? "open" : "close");
  }

  ngOnDestroy() {
    window.removeEventListener('resize', this.updateWindowWidth);
  }

  switchLanguage(lang: string) {
    this.currentLanguage = lang;
    this._translate.changeLang(lang);
  }
}
