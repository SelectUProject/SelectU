import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './components/layouts/main-layout/main-layout.component';
import LoginPageComponent from './components/pages/login-page/login-page.component';
import { ManageScholarshipsPageComponent } from './components/pages/manage-scholarships-page/manage-scholarships-page.component';
import RegisterPageComponent from './components/pages/register-page/register-page.component';
import LandingPageComponent from './components/pages/landing-page/landing-page.component';
import { AuthGuard } from './providers/auth.guard';
import { Role } from './models/Role';
import { FindScholarshipsComponent } from './components/pages/find-scholarships/find-scholarships.component';
import { MyApplicationsComponent } from './components/pages/my-applications/my-applications.component';
import { UserProfilePageComponent } from './components/pages/user-profile-page/user-profile-page.component';
import { UpdateUserProfilePageComponent } from './components/pages/update-user-profile-page/update-user-profile-page.component';

import UserInvitePageComponent from './components/pages/user-invite-page/user-invite-page.component';
import UserTablePageComponent from './components/pages/user-table-page/user-table-page.component';
import { CreateScholarshipApplicationPageComponent } from './components/pages/create-scholarship-application-page/create-scholarship-application-page.component';

//components

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: '', component: LandingPageComponent },
      { path: 'login', component: LoginPageComponent },
      {
        path: 'account',
        component: UserProfilePageComponent,
        canActivate: [AuthGuard],
      },
      {
        path: `find-scholarships`,
        component: FindScholarshipsComponent,
        canActivate: [AuthGuard],
        data: { role: Role.User },
      },
      {
        path: `create-scholarship-application`,
        component: CreateScholarshipApplicationPageComponent,
        canActivate: [AuthGuard],
        data: { role: Role.User },
      },
      {
        path: 'manage-scholarships',
        component: ManageScholarshipsPageComponent,
        canActivate: [AuthGuard],
        data: { role: Role.Staff },
      },
      { path: 'register', component: RegisterPageComponent },
      //     { path: '404', component: ErrorComponent },
      {
        path: 'applications',
        component: MyApplicationsComponent,
      },
      {
        path: 'update-account',
        canActivate: [AuthGuard],
        component: UpdateUserProfilePageComponent,
      },
      {
        path: 'invite-user',
        component: UserInvitePageComponent,
        canActivate: [AuthGuard],
        data: { role: [Role.Staff, Role.Admin] },
      },
      {
        path: 'view-users',
        component: UserTablePageComponent,
        canActivate: [AuthGuard],
        data: { role: [Role.Staff, Role.Admin] },
      },
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
