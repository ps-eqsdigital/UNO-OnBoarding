import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../user.service';
import { User } from '../user';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  error: string | null = null; 

  userForm: FormGroup;
  user: User = { uuid: '', name: '', email: '', password: '', picture : '', phone:'',role:0,id: 0 };
  constructor(private userService:UserService, private formBuilder: FormBuilder, private router:Router,  private route: ActivatedRoute) {
    this.userForm = this.formBuilder.group({
      name: [''],
      email: [''],
      password: [''],
      picture: [''],
      phone: ['']
    });
  }

  saveUser() {
    const editedUserData = this.userForm.value;
    const editedUser = {
      ...this.user,
      ...editedUserData
    };    
    this.userService.editUserData(this.user.uuid, editedUserData).subscribe(
      (response) => {
        this.error = null
        this.goBack();
      },
      (error) => {
        this.error = 'An error occurred. Please try again.'; 
      }
    );  
  }


  goBack(){
    this.router.navigate(['']);
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const userUuid = params['userUuid']; 
      this.userService.getUser(userUuid).subscribe(user => {
        this.user = user;
        this.userForm.patchValue(user);
      });
    });  }
}
