import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule,ReactiveFormsModule  } from '@angular/forms'; // Import FormsModule

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OptionsListComponent } from './options-list/options-list.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButtonComponent } from './button/button.component';
import { InputComponent } from './input/input.component';
import { SlideToggleComponent } from './slide-toggle/slide-toggle.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { CheckboxComponent } from './checkbox/checkbox.component';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { InsertUserComponent } from './insert-user/insert-user.component';
import { RouterModule } from '@angular/router';
import { ListUsersComponent } from './list-users/list-users.component';
import { EditUserComponent } from './edit-user/edit-user.component';
import { LoginComponent } from './login/login.component';
import { SidebarComponent } from './sidebar/sidebar.component';

import { MatInputModule } from '@angular/material/input';
import { UnoChartComponent } from './uno-chart/uno-chart.component';

@NgModule({
  declarations: [
    AppComponent,
    OptionsListComponent,
    ButtonComponent,
    InputComponent,
    SlideToggleComponent,
    CheckboxComponent,
    AppComponent,
    InsertUserComponent,
    ListUsersComponent,
    EditUserComponent,
    LoginComponent,
    SidebarComponent
    EditUserComponent,
    UnoChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSlideToggleModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatInputModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule
  ],
  providers: [],  
  bootstrap: [AppComponent],
    })
export class AppModule { }
