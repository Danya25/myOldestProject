import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import {RouterModule} from '@angular/router';
import {AppRoutingModule} from './app-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import { LoginComponent } from './auth/login/login.component';
import { RegistrComponent } from './auth/registr/registr.component';
import { AuthLayoutComponent } from './layout/auth-layout/auth-layout.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { AdvertisementComponent } from './content/advertisement/advertisement.component';
import { FeedbackComponent } from './content/feedback/feedback.component';
import { GaleryComponent } from './content/galery/galery.component';
import { NewsComponent } from './content/news/news.component';
import { OfficialComponent } from './content/official/official.component';
import { ReviewsComponent } from './content/reviews/reviews.component';
import { IndexComponent } from './content/index/index.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {HttpClientModule} from '@angular/common/http';
import {ToastrModule} from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegistrComponent,
    AuthLayoutComponent,
    MainLayoutComponent,
    AdvertisementComponent,
    FeedbackComponent,
    GaleryComponent,
    NewsComponent,
    OfficialComponent,
    ReviewsComponent,
    IndexComponent,

  ],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule,
    FormsModule,
    CommonModule,
    CarouselModule.forRoot(),
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    ToastrModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
