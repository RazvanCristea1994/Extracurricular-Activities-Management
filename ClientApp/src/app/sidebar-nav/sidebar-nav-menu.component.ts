import { Component, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { AuthenticationService } from "../authentication/authentication.service";

@Component({
  selector: 'app-sidebar-nav-menu',
  templateUrl: './sidebar-nav-menu.component.html',
  styleUrls: ['./sidebar-nav-menu.component.css']
})
export class SidebarNavMenuComponent implements OnInit {

  public isAdmin: Observable<boolean>;

  constructor(private authorizeService: AuthenticationService) {}

  ngOnInit() {
    this.isAdmin = this.authorizeService.isAdmin();
  }
}
