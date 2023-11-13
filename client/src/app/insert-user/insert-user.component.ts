import { Component } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './insert-user.component.html',
  styleUrls: ['./insert-user.component.css']
})
export class InsertUserComponent {
  user: User = {
    uuid:'' ,
    id: 0,
    name: '',
    password: '',
    email: '',
    phone: '',
    picture: '',
    role: 0
  };

  constructor(private userService:UserService){
  } 

  showSuccessMessage :boolean =false;
  errorMessage:boolean=false
  
  insertUsers():void{
    this.userService.insertUser(this.user).subscribe((data:any) => {
      if (data.isSuccess == true){
        this.showSuccessMessage=true;
        this.errorMessage=false
      }
      else{ 
        this.showSuccessMessage=false
        this.errorMessage=true
      }
    })
  }
}
