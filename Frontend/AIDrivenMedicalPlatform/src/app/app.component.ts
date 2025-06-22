import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import english from '../../public/i18n/en.json'
import arabic from '../../public/i18n/ar.json'
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Diagnosio';

  public currentLang = 'en';

  constructor(private translate: TranslateService){
    this.translate.setTranslation('en',english);
    this.translate.setTranslation('ar',arabic);

    this.translate.setDefaultLang('en');
  }

  
}
