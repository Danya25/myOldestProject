import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {AutoUnsubscribe} from 'ngx-auto-unsubscribe';
import {AuthenticateService} from '../../share/authenticate.service';
import {ToastrService} from 'ngx-toastr';
import {Route, Router} from '@angular/router';

@AutoUnsubscribe()
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {

  formAuth: FormGroup;

  constructor(private auth: AuthenticateService,
              private toastr: ToastrService,
              private router: Router) {
  }

  ngOnInit() {
    this.formAuth = new FormGroup({
      login: new FormControl(''),
      password: new FormControl('')
    });
  }

  onSubmit() {
    const user: User = {
      login: this.formAuth.value.login,
      password: this.formAuth.value.password
    };
    this.auth.isExistUser(user).subscribe(data => {
      if (data) {
        this.toastr.success('Вы успешно вошли', 'Login');
        this.auth.logIn();
        this.router.navigate(['/app/index']);
      } else {
        this.toastr.error('Ошибка входа', 'Login');
      }
    });

  }

  ngOnDestroy(): void {
  }
}
