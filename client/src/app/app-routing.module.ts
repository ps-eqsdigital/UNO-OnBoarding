import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListUsersComponent } from './list-users/list-users.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { InsertUserComponent } from './insert-user/insert-user.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth.guard';
import { ForgetPasswordComponent } from './forget-password/forget-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

const routes: Routes = [
  {path : 'login', component: LoginComponent},
  {path : 'users', component: ListUsersComponent, canActivate: [AuthGuard]},
  {path: 'edit-user/:userUuid', component: EditUserComponent, canActivate: [AuthGuard] },
  {path: 'insert', component:InsertUserComponent, canActivate: [AuthGuard]},
  {path : 'forget-password', component:ForgetPasswordComponent},
  {path : 'reset-password', component:ResetPasswordComponent},
  {path: '', redirectTo: '/login', pathMatch: 'full' }


]
;

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
