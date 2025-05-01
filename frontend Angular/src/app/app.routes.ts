

  //טעינה עצלה
  import { Routes } from '@angular/router';
  import { LoginComponent } from './login/login.component';
  
  export const routes: Routes = [
      { path: '', redirectTo: 'navbar', pathMatch: 'full' },   
      { path: 'login', component: LoginComponent },   
      { path: 'patient', loadComponent: () => import('./patient/patient.component').then(m => m.PatientComponent) },
      { path: 'patient-attribute', loadComponent: () => import('./patient-attributes/patient-attributes.component').then(m => m.PatientAttributesComponent) },
      { path: 'queue', loadComponent: () => import('./queue/queue.component').then(m => m.QueueComponent) },
      
      { path: '**', redirectTo: 'navbar' }
  ];