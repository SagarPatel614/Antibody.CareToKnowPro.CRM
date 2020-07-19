import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/service/services';
import { Router } from '@angular/router';
import { ILoginProfile as LoginProfile } from 'src/app/model/models';

declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTES: RouteInfo[] = [
    { path: '/dashboard', title: 'Dashboard',  icon: 'dashboard', class: '' },
    { path: '/doctor', title: 'Add HCP',  icon:'add', class: '' }, //bubble_chart
    { path: '/user-profile', title: 'User Profile',  icon:'person', class: '' },
    { path: '/recordcheck', title: 'Duplicate Check',  icon:'bubble_chart', class: '' },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  menuItems: any[];
  dashboard: RouteInfo;
  currentUser: LoginProfile;

  constructor(
    private router: Router,
    private authenticationService: AuthService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit() {
    this.menuItems = ROUTES.filter(menuItem => menuItem);
    this.dashboard = ROUTES.filter(menuItem => menuItem).find(a=>a.title === 'Dashboard');
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }

}
