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

  users: User[]=[];
  filteredUsers:User[]=[];

  searchQuery: string = '';
  sortValue: number = 1;

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
