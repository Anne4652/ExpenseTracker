import { Routes } from '@angular/router';
import { ExpenseFormComponent } from './components/expense-form/expense-form.component';
import { ExpenseListComponent } from './components/expense-list/expense-list.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'expenses', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'expenses', component: ExpenseListComponent, canActivate: [AuthGuard] },
  { path: 'expenses/new', component: ExpenseFormComponent, canActivate: [AuthGuard] },
  { path: 'expenses/edit/:id', component: ExpenseFormComponent, canActivate: [AuthGuard] }
];
