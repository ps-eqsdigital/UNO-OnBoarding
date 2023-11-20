import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../user.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent {

  email : string = '';
  errorMessage : boolean = false;

  constructor(private userService:UserService, private router: Router){
  }
  
  onSubmit() {
    this.userService.forgetPassword(this.email).subscribe(
      (data: any) => {
        this.errorMessage = false; 
        this.router.navigate(['/reset-password']);
      },
      (error) => {
        this.errorMessage = true; 
      }
    );
  }

}
