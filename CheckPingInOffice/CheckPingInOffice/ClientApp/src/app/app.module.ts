import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';

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
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTabsModule,
    MatDividerModule,
    MatButtonModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'ip', component: ChangeIpComponent, pathMatch: 'full'},
    ]),
    BrowserAnimationsModule
  ],
  providers: [PingService],
  bootstrap: [AppComponent]
})
export class AppModule { }
