import { Component } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  
  users: User[]=[];
  filteredUsers=this.users;

  searchQuery: string = '';
  sortValue: number = 1;

  constructor(private userService:UserService){
  }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers():void{
    this.userService.getUsers().subscribe(users=>this.users=users)
  }
  
  getFilteredUsers(): void {
    this.userService.getFilteredUsers(this.searchQuery, this.sortValue).subscribe((users) => {
      this.filteredUsers = users;
    });
  }
}
