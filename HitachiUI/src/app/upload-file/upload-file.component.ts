import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AppServiceService} from "../app-service.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.css']
})
export class UploadFileComponent implements OnInit{

  formGroup: FormGroup;

  constructor(private appService: AppServiceService,
              private router: Router,
              private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.formGroup = new FormGroup({
      sender: new FormControl({
        value: null,
        disabled: false
      }, [Validators.required]),
      password: new FormControl({
        value: null,
        disabled: false
      }, [Validators.required]),
      receiver: new FormControl({
        value: null,
        disabled: false
      }, [Validators.required]),
      file: new FormControl({
        value: null,
        disabled: false
      }),
    });
  }

  handleFileInput(event: any) {
    const file: File = event.target.files[0];
    this.formGroup.get('file').setValue(file);
    console.log(this.formGroup);
  }

  submit() {
    this.formGroup.markAllAsTouched();
    if (this.formGroup.valid) {
      let data = this.formGroup.getRawValue();

      const formData = new FormData();
      formData.append('sender', data['sender']);
      formData.append('password', data['password']);
      formData.append('receiver', data['receiver']);
      formData.append('file', data['file']);

      console.log(data);
      this.appService.postUploadData(formData).subscribe(data => {
        this.toastr.success('Uploaded');
        this.router.navigate(['success']);
      }, error => {
        this.toastr.error('Failed to upload the data!');
      });
    } else {
      this.toastr.error('Fields are not correctly filled!');
    }
  }
}
