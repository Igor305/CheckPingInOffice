import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

import { PingService } from './services/ping.service';
import { ChangeIpComponent } from './change-ip/change-ip.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ChangeIpComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'ip', component: ChangeIpComponent, pathMatch: 'full'},
    ])
  ],
  providers: [PingService],
  bootstrap: [AppComponent]
})
export class AppModule { }
