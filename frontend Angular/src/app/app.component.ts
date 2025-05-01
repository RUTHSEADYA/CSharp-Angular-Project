
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from '@angular/material/dialog';
import { PatientComponent } from './patient/patient.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent,NavBarComponent,HttpClientModule,MatDialogModule
    ,PatientComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'emergency-room';
  showPatientButtons = false;

   constructor() {}
   
  handleTogglePatients(event: boolean) {
    this.showPatientButtons = event;
  }

}



