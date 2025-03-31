import { Component } from '@angular/core';
import { ChildrenOutletContexts, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { trigger, style, animate, transition, query } from '@angular/animations';

@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet, CommonModule],
  templateUrl: './auth-layout.component.html',
  styleUrl: './auth-layout.component.scss',
  animations: [
    trigger('routerFadeIn', [
      transition('* <=> *', [
        query(':enter', [
          style({ opacity: 0 }),
          animate('0.5s ease-in-out', style({ opacity: 1 }))
        ], {optional : true})
      ])
    ])
  ],
})
export class AuthLayoutComponent {

  constructor(private context : ChildrenOutletContexts){}

  getRouterURL(){
    return this.context.getContext('primary')?.route?.url;
  }

}
