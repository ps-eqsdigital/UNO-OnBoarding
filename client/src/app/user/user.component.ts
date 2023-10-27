import { Component } from '@angular/core';
import { User } from '../user';
import { UserService } from '../user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  
  users: User[]=[];
  filteredUsers:User[]=[];

  searchQuery: string = '';
  sortValue: number = 1;

  constructor(private userService:UserService, private router: Router){
  }

  ngOnInit(): void {
    this.getUsers();
    this.getFilteredUsers();
  }

  getUsers():void{
    this.userService.getUsers().subscribe(users=>this.users=users)
  }
  
  getFilteredUsers(): void {
    this.userService.getFilteredUsers(this.searchQuery, this.sortValue).subscribe((users) => {
      this.filteredUsers = users;
    });
  } 

  editUser(uuid:string,data:User):void{
    this.userService.editUserData(uuid,data).subscribe((data)=>console.log(uuid));
  }
  navigateToEditPage(userUuid: string) {
    this.router.navigate(['/edit-user', userUuid]);
  }
}
