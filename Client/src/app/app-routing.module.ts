import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './components/layouts/main-layout/main-layout.component';
import LoginPageComponent from './components/pages/login-page/login-page.component';
//components

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      //     // { path: '', component: HomePageComponent },
      //     {
      //       path: 'dashboard',
      //       canActivate: [AuthGuard],
      //       data: { role: Role.User },
      //       children: [{ path: '', component: DashboardComponent }],
      //     },
      { path: 'login', component: LoginPageComponent },
      //     { path: 'signup', component: LandingComponent },
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
