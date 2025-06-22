import { Router } from '@angular/router';
import { Component, ElementRef, inject, OnInit, QueryList, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { TranslationService } from '../../Core/Services/translation.service';


@Component({
  selector: 'app-main-page',
  imports: [CommonModule, TranslateModule],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent implements OnInit {

  currentLanguage: string = 'en';
  showLang: boolean = false;

  private readonly _translate = inject(TranslationService)

  constructor(private _router: Router) { }

  ngOnInit(): void {
    if (localStorage.getItem('lang') == 'ar') {
    this.currentLanguage = 'ar';
  }
  }


  /* 🟢Section_____________________________________________________________________________________________1🟢 */
  onGetStarted(): void {
    this._router.navigate(["/register"]);
  }

  onWatchDemo(): void {
    console.log('Show demo');
  }


  toggleLanguage() {
    this.showLang = !this.showLang;
  }

  switchLanguage(lang: string) {
    this.currentLanguage = lang;
    this._translate.changeLang(lang);
  }

}