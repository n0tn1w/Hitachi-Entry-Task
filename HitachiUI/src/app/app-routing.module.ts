import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {UploadFileComponent} from "./upload-file/upload-file.component";
import {SuccessUploadFileComponent} from "./success-upload-file/success-upload-file.component";

const routes: Routes = [
  {
    path: '',
    component: UploadFileComponent
  },
  {
    path: 'success',
    component: SuccessUploadFileComponent
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
