import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListUsersComponent } from './list-users/list-users.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { InsertUserComponent } from './insert-user/insert-user.component';
import { ButtonComponent } from './button/button.component';
import { AppComponent } from './app.component';
import { UnoChartComponent } from './uno-chart/uno-chart.component';

const routes: Routes = [
  {path : '', component: AppComponent},
  {path: 'edit-user/:userUuid', component: EditUserComponent },
  {path: 'insert', component:InsertUserComponent}

]
;

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
