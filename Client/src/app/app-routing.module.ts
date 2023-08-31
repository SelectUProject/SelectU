import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './components/layouts/main-layout/main-layout.component';
import LoginPageComponent from './components/pages/login-page/login-page.component';
import { ManageScholarshipsPageComponent } from './components/pages/manage-scholarships-page/manage-scholarships-page.component';
import RegisterPageComponent from './components/pages/register-page/register-page.component';
import LandingPageComponent from './components/pages/landing-page/landing-page.component';
//components

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', component: LandingPageComponent },
      //     {
      //       path: 'dashboard',
      //       canActivate: [AuthGuard],
      //       data: { role: Role.User },
      //       children: [{ path: '', component: DashboardComponent }],
      //     },
      { path: 'login', component: LoginPageComponent },
      {
        path: 'manage-scholarships',
        component: ManageScholarshipsPageComponent,
      },
      //     { path: 'signup', component: LandingComponent },
      { path: 'register', component: RegisterPageComponent },
      //     { path: '404', component: ErrorComponent },
    ],
  },
  // {
  //   path: 'admin',
  //   component: AdminLayoutComponent,
  //   children: [],
  // },
  // { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      initialNavigation: 'enabledBlocking',
      scrollPositionRestoration: 'top',
      anchorScrolling: 'enabled',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
