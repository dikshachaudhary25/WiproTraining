import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { PropertiesComponent } from './pages/properties/properties';
import { AddPropertyComponent } from './pages/add-property/add-property';
import { PropertyDetailsComponent } from './pages/property-details/property-details';
import { MyReservationsComponent } from './pages/my-reservations/my-reservations';

export const routes: Routes = [
  { path: '', component: HomeComponent },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'properties', component: PropertiesComponent },
  { path: 'properties/:id', component: PropertyDetailsComponent },

  { path: 'add-property', component: AddPropertyComponent },
  { path: 'edit-property/:id', component: AddPropertyComponent },

  { path: 'my-reservations', component: MyReservationsComponent },

  { path: '**', redirectTo: '' }
];