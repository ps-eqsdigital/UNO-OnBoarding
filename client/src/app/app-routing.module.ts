import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserComponent } from './user/user.component';

const routes: Routes = [
  {path : '', component: UserComponent},
  { path: 'edit-user/:userUuid', component: UserEditComponent },

]
;

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
