import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../authentication/authentication.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  public isAuthenticated: Observable<boolean>;

  constructor(
    private authorizeService: AuthenticationService,
  ) { }

  ngOnInit() {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
  }

  logout() {
    this.authorizeService.removeToken();
    window.location.href = '/';
  }
}
