import { NgModule } from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './auth/login/login.component';
import {RegistrComponent} from './auth/registr/registr.component';
import {AuthLayoutComponent} from './layout/auth-layout/auth-layout.component';
import {MainLayoutComponent} from './layout/main-layout/main-layout.component';
import {AdvertisementComponent} from './content/advertisement/advertisement.component';
import {FeedbackComponent} from './content/feedback/feedback.component';
import {GaleryComponent} from './content/galery/galery.component';
import {NewsComponent} from './content/news/news.component';
import {OfficialComponent} from './content/official/official.component';
import {ReviewsComponent} from './content/reviews/reviews.component';
import {IndexComponent} from './content/index/index.component';

const routes: Routes = [{
  path: 'auth', component: AuthLayoutComponent, children: [
    {path: 'login', component: LoginComponent},
    {path: 'registration', component: RegistrComponent},
  ]
},
  {
    path: 'app', component: MainLayoutComponent, children: [
      {path: '', redirectTo: 'index', pathMatch: 'full'},
      {path: 'index', component: IndexComponent},
      {path: 'advertisement', component: AdvertisementComponent},
      {path: 'feedback', component: FeedbackComponent},
      {path: 'galery', component: GaleryComponent},
      {path: 'news', component: NewsComponent},
      {path: 'official', component: OfficialComponent},
      {path: 'reviews', component: ReviewsComponent}
    ]
  },
  {path: '**', redirectTo: 'app/index', pathMatch: 'full'}
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
