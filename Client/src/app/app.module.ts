import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

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
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MdbValidationModule } from 'mdb-angular-ui-kit/validation';
import { MdbCollapseModule } from 'mdb-angular-ui-kit/collapse';
import {
  SocialLoginModule,
  GoogleSigninButtonModule,
  GoogleLoginProvider,
  SocialAuthServiceConfig,
} from '@abacritt/angularx-social-login';

// Custom components
import NavbarComponent from './components/layouts/navbar/navbar.component';
import LoginPageComponent from './components/pages/login-page/login-page.component';
import LoginFormComponent from './components/shared/login-form/login-form.component';
import RegisterFormComponent from './components/shared/register-form/register-form.component';
import RegisterPageComponent from './components/pages/register-page/register-page.component';

@NgModule({
  declarations: [
    AppComponent,
    MainLayoutComponent,
    NavbarComponent,
    LoginPageComponent,
    LoginFormComponent,
    RegisterFormComponent,
    RegisterPageComponent,
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
    SocialLoginModule,
    GoogleSigninButtonModule,
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
            provider: new GoogleLoginProvider(
              '916817943783-uep7ecjsr44mroo06ig665nmsm0aad9t.apps.googleusercontent.com'
            ),
          },
        ],
      } as SocialAuthServiceConfig,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
