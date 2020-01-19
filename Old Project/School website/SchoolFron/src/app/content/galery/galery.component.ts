import {Component, OnDestroy, OnInit} from '@angular/core';
import {ImagesService} from '../../share/images.service';
import {AutoUnsubscribe} from 'ngx-auto-unsubscribe';

@AutoUnsubscribe()
@Component({
  selector: 'app-galery',
  templateUrl: './galery.component.html',
  styleUrls: ['./galery.component.css']
})
export class GaleryComponent implements OnInit, OnDestroy {
  img: Array<string>;

  constructor(private imgService: ImagesService) {
  }

  ngOnInit() {
    this.imgService.getImg().subscribe(data => {
      this.img = data;
    });
  }

  ngOnDestroy() {
  }

}
