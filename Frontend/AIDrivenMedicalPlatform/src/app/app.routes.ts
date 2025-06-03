
import { Routes } from '@angular/router';
import { AuthLayoutComponent } from './Layouts/auth-layout/auth-layout.component';
import { RegisterComponent } from './Components/auth/register/register.component';
import { LoginComponent } from './Components/auth/login/login.component';
import { BlankLayoutComponent } from './Layouts/blank-layout/blank-layout.component';
import { authGuard } from './Core/Guards/auth.guard';
import { stopLoggedInUserGuard } from './Core/Guards/stop-logged-in-user.guard';
import { DoctorProfileComponent } from './Components/doctor/doctor-profile/doctor-profile.component';
import { PatientProfileComponent } from './Components/patient/patient-profile/patient-profile.component';
import { AdminProfileComponent } from './Components/admin/admin-profile/admin-profile.component';
import { PatientDashboardComponent } from './Components/patient/patient-dashboard/patient-dashboard.component';
import { AdminDashboardComponent } from './Components/admin/admin-dashboard/admin-dashboard.component';
import { DoctorDashboardComponent } from './Components/doctor/doctor-dashboard/doctor-dashboard.component';
import { DoctorAppointmentsComponent } from './Components/doctor/doctor-appointments/doctor-appointments.component';
import { ForbiddenComponent } from './Components/forbidden/forbidden.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { claimReq } from './Core/utils/claimReq-utils';
import { PatientAppointmentComponent } from './Components/patient/patient-appointment/patient-appointment.component';
import { MainPageComponent } from './Components/main-page/main-page.component';

export const routes: Routes = [
    {path: '', component: AuthLayoutComponent, canActivate:[stopLoggedInUserGuard], children:[
        {path: "", redirectTo: "home", pathMatch: 'full'},
        {path: "home", component: MainPageComponent },
        {path: "register", loadComponent: () => import("./Components/auth/register/register.component").then((c) => c.RegisterComponent)},
        {path: "login", loadComponent: () => import("./Components/auth/login/login.component").then((c) => c.LoginComponent)}
    ]},
    {path:'', component: BlankLayoutComponent, canActivate:[authGuard], canActivateChild:[authGuard] , children:[
        {path: "", redirectTo: "AdminDashboard", pathMatch: 'full'},

        {path: 'AdminProfile', component: AdminProfileComponent, data: {claimReq : claimReq.adminOnly}},
        {path: 'AdminDashboard', component: AdminDashboardComponent, data: {claimReq : claimReq.adminOnly}},

        {path: 'DoctorProfile', component: DoctorProfileComponent, data: {claimReq : claimReq.doctorOnly}},
        {path: 'DoctorDashboard', component: DoctorDashboardComponent, data: {claimReq : claimReq.doctorOnly}},
        {path: 'DoctorAppointments', component: DoctorAppointmentsComponent, data: {claimReq : claimReq.doctorOnly}},


        {path: 'PatientProfile', component: PatientProfileComponent, data: {claimReq : claimReq.patientOnly}},
        {path: 'PatientDashboard', component: PatientDashboardComponent, data: {claimReq : claimReq.patientOnly}},
        {path: 'PatientAppointment', component: PatientAppointmentComponent, data: {claimReq : claimReq.patientOnly}},
        {path: 'forbidden', component: ForbiddenComponent},

        {path: "feedback", loadComponent: () => import("./Components/feedback/feedback.component").then((c) => c.FeedbackComponent) },
        {path: "notification", loadComponent: () => import("./Components/notification/notification.component").then((c) => c.NotificationComponent) }
    ]},
    {path: "**", component: NotFoundComponent }
];
