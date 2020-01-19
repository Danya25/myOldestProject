import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImagesService {

  constructor(private http: HttpClient) {
  }

  getImg(): Observable<Array<string>> {
    const httpHeader = new HttpHeaders({'Content-type': 'text/html'});
    return this.http.get<Array<string>>('http://localhost:4200/api/Img/getimg');
  }

}
