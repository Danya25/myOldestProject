import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {AuthenticateService} from '../../share/authenticate.service';
import {AutoUnsubscribe} from 'ngx-auto-unsubscribe';
import {ToastrService} from 'ngx-toastr';

@AutoUnsubscribe()
@Component({
  selector: 'app-registr',
  templateUrl: './registr.component.html',
  styleUrls: ['./registr.component.css']
})
export class RegistrComponent implements OnInit, OnDestroy {

  formReg: FormGroup;

  constructor(private auth: AuthenticateService,
              private toastr: ToastrService) {
  }

  ngOnInit() {
    this.formReg = new FormGroup({
      login: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });
  }

  onSubmit() {
    let user: User = {
      login: this.formReg.value.login,
      password: this.formReg.value.password
    };
    this.auth.userRegistration(user).subscribe(date => {
      if (date) {
        this.toastr.success('Success Registration', 'Registration');
      } else {
        this.toastr.error('Error registration', 'Registration');
      }
    });
  }

  ngOnDestroy(): void {
  }

}
