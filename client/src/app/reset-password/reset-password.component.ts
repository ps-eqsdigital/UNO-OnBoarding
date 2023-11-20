import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../user.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent {

  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  passwordsNotMatch: boolean = false;
  errorMessage : boolean = false;

  constructor(private userService: UserService, private router: Router) {}

  onSubmit() {
    if (this.newPassword !== this.confirmPassword) {
      this.passwordsNotMatch = true;
      return;
    }

    this.userService.resetPassword(this.token, this.newPassword).subscribe(
      (data: any) => {
        this.errorMessage=false
        this.router.navigate(['/login']); 
      },
      (error) => {
        this.errorMessage=true
      }
    );
  }
}
