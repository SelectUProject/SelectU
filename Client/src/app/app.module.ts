import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { environment } from 'src/environments/environment';

import { DatePipe } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { MainLayoutComponent } from './components/layouts/main-layout/main-layout.component';
import { AuthGuard } from './providers/auth.guard';
import { TokenInterceptor } from './providers/token.interceptor';
import { MdbTabsModule } from 'mdb-angular-ui-kit/tabs';
import { MdbFormsModule } from 'mdb-angular-ui-kit/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ManageScholarshipsPageComponent } from './components/pages/manage-scholarships-page/manage-scholarships-page.component';
import { ShortViewScholarshipsComponent } from './components/shared/short-view-scholarships/short-view-scholarships.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MdbValidationModule } from 'mdb-angular-ui-kit/validation';
import { MdbCollapseModule } from 'mdb-angular-ui-kit/collapse';
import { MdbDropdownModule } from 'mdb-angular-ui-kit/dropdown';
import { DragDropModule, CdkDragHandle } from '@angular/cdk/drag-drop';
import {
  SocialLoginModule,
  GoogleSigninButtonModule,
  GoogleLoginProvider,
  SocialAuthServiceConfig,
} from '@abacritt/angularx-social-login';
import { MdbModalModule } from 'mdb-angular-ui-kit/modal';

// Custom components
import NavbarComponent from './components/layouts/navbar/navbar.component';
import LoginPageComponent from './components/pages/login-page/login-page.component';
import LoginFormComponent from './components/shared/login-form/login-form.component';
import RegisterPageComponent from './components/pages/register-page/register-page.component';
import RegisterFormComponent from './components/shared/register-form/register-form.component';
import LandingPageComponent from './components/pages/landing-page/landing-page.component';
import { FindScholarshipsComponent } from './components/pages/find-scholarships/find-scholarships.component';
import { MyApplicationsComponent } from './components/pages/my-applications/my-applications.component';
import { ShortViewMyApplicationsComponent } from './components/shared/short-view-my-applications/short-view-my-applications.component';
import { EmptyScholarshipsComponent } from './components/shared/empty-scholarships/empty-scholarships.component';
import { ScholarshipSearchFormComponent } from './components/shared/scholarship-search-form/scholarship-search-form.component';
import { CreateScholarshipApplicationPageComponent } from './components/pages/create-scholarship-application-page/create-scholarship-application-page.component';
import { LongViewScholarshipsComponent } from './components/shared/long-view-scholarships/long-view-scholarships.component';
import { ScholarshipApplicationFormComponent } from './components/shared/scholarship-application-form/scholarship-application-form.component';
import { ScholarshipApplicationSearchFormComponent } from './components/shared/scholarship-application-search-form/scholarship-application-search-form.component';
import UserInviteFormComponent from './components/shared/user-invite-form/user-invite-form.component';
import UserTablePageComponent from './components/pages/user-table-page/user-table-page.component';
import UserUpdateModalComponent from './components/shared/user-update-modal/user-update-modal.component';
import UserInvitePageComponent from './components/pages/user-invite-page/user-invite-page.component';
import UserTableComponent from './components/shared/user-table/user-table.component';
import { NgbdSortableHeader } from './components/shared/user-table/ngbd-sortable-header/ngbd-sortable-header.component';
import ViewDetailsModalComponent from './components/shared/view-details-modal/view-details-modal.component';
import { ViewApplicationDetailModalComponent } from './components/shared/view-application-detail-modal/view-application-detail-modal.component';
import { CreateScholarshipComponent } from './components/pages/create-scholarship/create-scholarship.component';
import { DragAndDropService } from './services/drag-and-drop/drag-and-drop.service';
import { FormSectionComponent } from './components/pages/drag-and-drop-form-creator/form-section/form-section.component';
import { FormSectionEditDialogBoxComponent } from './components/pages/drag-and-drop-form-creator/form-section-edit-dialog-box/form-section-edit-dialog-box.component';
import { FormCreatorAreaComponent } from './components/pages/drag-and-drop-form-creator/form-creator-area/form-creator-area.component';
import { FormSectionListSidebarComponent } from './components/pages/drag-and-drop-form-creator/form-section-list-sidebar/form-section-list-sidebar.component';
import { ScholarshipFormSectionListService } from './services/scholarship-form-section-list/scholarship-form-section-list.service';
import { ToastsContainerComponent } from './components/shared/toasts-container/toasts-container.component';

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    NavbarComponent,
    LoginPageComponent,
    LoginFormComponent,
    ManageScholarshipsPageComponent,
    ShortViewScholarshipsComponent,
    RegisterPageComponent,
    RegisterFormComponent,
    LandingPageComponent,
    FindScholarshipsComponent,
    MyApplicationsComponent,
    ShortViewMyApplicationsComponent,
    EmptyScholarshipsComponent,
    ScholarshipSearchFormComponent,
    CreateScholarshipApplicationPageComponent,
    LongViewScholarshipsComponent,
    ScholarshipApplicationFormComponent,
    ScholarshipApplicationSearchFormComponent,
    UserInvitePageComponent,
    UserInviteFormComponent,
    UserTablePageComponent,
    UserUpdateModalComponent,
    UserTableComponent,
    ViewDetailsModalComponent,
    ViewApplicationDetailModalComponent
    CreateScholarshipComponent,
    FormSectionComponent,
    FormSectionEditDialogBoxComponent,
    FormCreatorAreaComponent,
    FormSectionListSidebarComponent,
    ToastsContainerComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'serverApp' }),
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    MdbTabsModule,
    MdbFormsModule,
    BrowserAnimationsModule,
    BsDatepickerModule,
    MdbValidationModule,
    MdbCollapseModule,
    MdbDropdownModule,
    SocialLoginModule,
    GoogleSigninButtonModule,
    MdbModalModule,
    NgbdSortableHeader,
    DragDropModule,
    CdkDragHandle,
  ],
  providers: [
    DatePipe,
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      multi: true,
      useClass: TokenInterceptor,
    },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(environment.googleClientId),
          },
        ],
      } as SocialAuthServiceConfig,
    },
    DragAndDropService,
    ScholarshipFormSectionListService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
