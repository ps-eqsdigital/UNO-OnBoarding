import { Component } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  user: User = {
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

  insertUsers():void{
    this.userService.insertUser(this.user).subscribe((data:any) => {
      if (data.isSuccess == true){
        this.showSuccessMessage=true;
      }
      else{
        this.showSuccessMessage=false
      }
    })
  }
}
