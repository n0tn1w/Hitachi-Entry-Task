import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessUploadFileComponent } from './success-upload-file.component';

describe('SuccessUploadFileComponent', () => {
  let component: SuccessUploadFileComponent;
  let fixture: ComponentFixture<SuccessUploadFileComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SuccessUploadFileComponent]
    });
    fixture = TestBed.createComponent(SuccessUploadFileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
