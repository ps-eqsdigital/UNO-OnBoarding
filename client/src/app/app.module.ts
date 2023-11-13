import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule,ReactiveFormsModule  } from '@angular/forms'; // Import FormsModule

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { HttpClientModule } from '@angular/common/http';
import { UserEditComponent } from './user-edit/user-edit.component';
import { RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent
    AppComponent,
    UserComponent,
    UserEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
