import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ILoginProfile as LoginProfile } from 'src/app/model/models';
import { AuthService } from '../service/auth.service';

@Component({
  selector: 'app-footer1',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  isExpanded = false;
  currentUser: LoginProfile;
  title = 'Antibody CareToKnowPro';

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor(
    private router: Router,
    private authenticationService: AuthService
  ) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }
}
