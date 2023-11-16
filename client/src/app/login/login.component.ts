import { Component } from '@angular/core';
import { LoginService } from '../login.service';
import { Route } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage:boolean = false;
  constructor(private loginService:LoginService){
  }

  onSubmit() {
    this.loginService.login(this.email, this.password).subscribe(
      (data: any) => {
        this.errorMessage = false; 
      },
      (error) => {
        this.errorMessage = true; 
      }
    );
  }
}