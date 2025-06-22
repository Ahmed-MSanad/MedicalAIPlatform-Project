import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { BackgroundLayoutComponent } from "../../../Layouts/background-layout/background-layout.component";
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-admin-dashboard',
  imports: [RouterModule, BackgroundLayoutComponent, TranslateModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {

}
