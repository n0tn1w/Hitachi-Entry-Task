import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../environments/environment";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AppServiceService {

  backEndURL = environment.BEUrl;

  constructor(private  httpClient: HttpClient) { }

  postUploadData(data: any): Observable<boolean> {
    console.log(data);
    const url=`${this.backEndURL}/Space/Upload`;
    return this.httpClient.post<boolean>(url, data);
  }

}
