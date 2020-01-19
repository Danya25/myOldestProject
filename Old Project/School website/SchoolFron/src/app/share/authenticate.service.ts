import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {

  isLogin: boolean = false;

  constructor(private http: HttpClient) {
  }

  logIn() {
    this.isLogin = true;
  }

  userRegistration(user: User): Observable<boolean> {
    // @ts-ignore
    return this.http.post('http://localhost:4200/api/Auth/createuser', user);
  }

  isExistUser(user: User): Observable<boolean> {
    // @ts-ignore
    return this.http.post('http://localhost:4200/api/Auth/checkuser', user);

  }

}
