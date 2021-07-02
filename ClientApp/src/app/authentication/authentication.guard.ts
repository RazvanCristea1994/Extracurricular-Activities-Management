import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { AuthenticationService } from "./authentication.service";

@Injectable({
  providedIn: 'root'
})

export class AuthenticationGuardService implements CanActivate {

  constructor(private router: Router, private authService: AuthenticationService) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

    return this.authService
      .isAuthenticated().pipe(map((result) => {
        if (result == true) {
          return result;
        }

        this.router.navigate(['/']);
        return false;
      }));
  }
}
