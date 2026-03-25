import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { AppComponent } from './app';
import { MenuComponent } from './menu/menu';
import { HomeComponent } from './home/home';
import { MyHobbiesComponent } from './my-hobbies/my-hobbies';
import { NewHobbyComponent } from './new-hobby/new-hobby';
import { FavHobbyComponent } from './fav-hobby/fav-hobby';
import { LoginComponent } from './component/login/login';
import { RegisterComponent } from './component/register/register';

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    FormsModule,
    CommonModule,
    HttpClientModule,
    AppComponent,
    MenuComponent,
    HomeComponent,
    MyHobbiesComponent,
    NewHobbyComponent,
    FavHobbyComponent,
    LoginComponent,
    RegisterComponent,
    AppRoutingModule
  ]
})
export class AppModule {}