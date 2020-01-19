import { Component, OnInit } from '@angular/core';
import {AuthenticateService} from '../../share/authenticate.service';

@Component({
  selector: 'app-feedback',
  templateUrl: './feedback.component.html',
  styleUrls: ['./feedback.component.css']
})
export class FeedbackComponent implements OnInit {

  constructor(private auth: AuthenticateService) { }

  ngOnInit() {
  }

}
