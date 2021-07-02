import { Component, ViewEncapsulation } from "@angular/core";
import { AuthenticationResponse } from "../authentication/authentication.model";
import { AuthenticationService } from "../authentication/authentication.service";
import { Login } from "./login.model";
import { LoginService } from "./login.service";

@Component({
  selector: 'app-login',
  templateUrl: 'login.component.html',
  encapsulation: ViewEncapsulation.None,
})

export class NewLoginComponent {

  loginData = new Login();
  errorMessages = [];

  constructor(
    private loginService: LoginService,
    private authService: AuthenticationService
  ) { }

  logIn() {
    this.loginService.login(this.loginData)
      .subscribe(
        (response: AuthenticationResponse) => {
          this.authService.saveToken(response.token);
          window.location.href = '/';
        },
        error => this.errorMessages = error.error.errors
      );
  }
}
