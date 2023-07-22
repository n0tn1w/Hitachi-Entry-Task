import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import {RouterModule} from "@angular/router";
import {CommonModule} from "@angular/common";
import {HttpClientModule} from "@angular/common/http";
import { UploadFileComponent } from './upload-file/upload-file.component';
import {AppRoutingModule} from "./app-routing.module";
import {ReactiveFormsModule} from "@angular/forms";
import {NgToastModule} from "ng-angular-popup";
import { SuccessUploadFileComponent } from './success-upload-file/success-upload-file.component';
import {ToastrModule} from "ngx-toastr";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";

@NgModule({
  declarations: [
    AppComponent,
    UploadFileComponent,
    SuccessUploadFileComponent,
    ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    RouterModule,
    HttpClientModule,
    CommonModule,
    ReactiveFormsModule,
    ToastrModule.forRoot({
      positionClass: 'toast-top-right',
 //     closeButton: true,
      progressBar: true
    }),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
