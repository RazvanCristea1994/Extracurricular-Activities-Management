import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RegistrationComponent } from './register/registration.component';
import { NewLoginComponent } from './login/login.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthorizeInterceptor } from '../api-authorization/authorize.interceptor';
import { AuthorizeGuard } from '../api-authorization/authorize.guard';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AccountComponent } from './my-account/account.component';
import { SidebarNavMenuComponent } from './sidebar-nav/sidebar-nav-menu.component';
import { AdminUserAddComponent } from './users/admin/user-add/user-add.component';
import { AdminUserEditComponent } from './users/admin/user-edit/user-edit.component';
import { AuthRoleGuardService } from './authentication/authentication.role.guard';
import { AuthenticationGuardService } from './authentication/authentication.guard';
import { AdminUsersComponent } from './users/admin/users.componenet';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RegistrationComponent,
    NewLoginComponent,
    AccountComponent,
    SidebarNavMenuComponent,
    AdminUserEditComponent,
    AdminUserAddComponent,
    AdminUsersComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'register', component: RegistrationComponent },
      { path: 'login', component: NewLoginComponent },
      { path: 'account', component: AccountComponent, canActivate: [AuthenticationGuardService] },
      { path: 'users', component: AdminUsersComponent, canActivate: [AuthenticationGuardService, AuthRoleGuardService] },
      { path: 'user/edit/:id', component: AdminUserEditComponent, canActivate: [AuthenticationGuardService, AuthRoleGuardService] },
      { path: 'user/add', component: AdminUserAddComponent, canActivate: [AuthenticationGuardService, AuthRoleGuardService] },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
