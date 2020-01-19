import {Component, OnDestroy, OnInit} from '@angular/core';
import {AuthenticateService} from '../../share/authenticate.service';

@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css']
})
export class MainLayoutComponent implements OnInit {
  isLogin: boolean;

  constructor(private auth: AuthenticateService) { }


  ngOnInit() {
    this.isLogin = this.auth.isLogin;
  }

}
