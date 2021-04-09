import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;
  maxDate: Date;
  validationErrors: string[] = [];

  constructor( private accountService: AccountService
             , private toastr: ToastrService
             , private fb: FormBuilder
             , private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm(){
    // this.registerForm = new FormGroup({
    //   username: new FormControl("", Validators.required),
    //   password: new FormControl("", [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl("", [Validators.required, this.matchValues("password")])
    // });
    this.registerForm = this.fb.group({
      username: ["", Validators.required],
      gender: ["male"],
      dateOfBirth: ["", Validators.required],
      knownAs: ["", Validators.required],
      city: ["", Validators.required],
      country: ["", Validators.required],
      password: ["", [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ["", [Validators.required, this.matchValues("password")]]
    });

    //The validators above cannot handle the condition of changing the password after entering confirmPassword
    this.registerForm.controls.password.valueChanges.subscribe(() =>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn{
    return(control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatch: true};
    }
  }

  register(){
    // this.accountService.register(this.model).subscribe(response =>{
    this.accountService.register(this.registerForm.value).subscribe(response =>{
      // this.router.navigateByUrl("/members");
      console.log(this.registerForm.value);
      // this.cancel();
    }, error =>{
      this.validationErrors = error;
      // console.log(error);
      // this.toastr.error(error.error);
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
