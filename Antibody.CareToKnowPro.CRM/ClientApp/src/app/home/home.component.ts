import { Component, OnInit, OnDestroy } from '@angular/core';

import { AuthService } from '../service/auth.service';
import { ILoginProfile as LoginProfile } from 'src/app/model/models';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  currentUser: LoginProfile;
  currentUserSubscription: Subscription;

  constructor(
    private authenticationService: AuthService
  ) {
    this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
      this.currentUser = user;
    });
  }

  ngOnDestroy() {
    // unsubscribe to ensure no memory leaks
    this.currentUserSubscription.unsubscribe();
  }
}
