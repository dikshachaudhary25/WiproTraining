import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { PropertiesComponent } from './pages/properties/properties';
import { AddPropertyComponent } from './pages/add-property/add-property';
import { PropertyDetailsComponent } from './pages/property-details/property-details';
import { MyReservationsComponent } from './pages/my-reservations/my-reservations';
import { OwnerReservationsComponent } from './pages/owner-reservations/owner-reservations';
import { MessagesComponent } from './pages/messages/messages';
import { MyPropertiesComponent } from './pages/my-properties/my-properties';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { WishlistComponent } from './pages/wishlist/wishlist';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  { path: 'properties', component: PropertiesComponent },
  { path: 'properties/:id', component: PropertyDetailsComponent },

  { path: 'add-property', component: AddPropertyComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Owner' } },
  { path: 'edit-property/:id', component: AddPropertyComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Owner' } },

  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Owner' } },
  { path: 'my-properties', component: MyPropertiesComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Owner' } },
  { path: 'owner-reservations', component: OwnerReservationsComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Owner' } },
  
  { path: 'my-reservations', component: MyReservationsComponent, canActivate: [AuthGuard] },
  { path: 'wishlist', component: WishlistComponent, canActivate: [AuthGuard] },
  { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard] },

  { path: '**', redirectTo: '' }
];