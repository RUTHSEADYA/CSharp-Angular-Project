
// import { Component } from '@angular/core';
// import { MatCardModule } from '@angular/material/card';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatInputModule } from '@angular/material/input';
// import { MatButtonModule } from '@angular/material/button';
// import {FormBuilder, FormControl, FormGroup,AbstractControl , Validators, ReactiveFormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';
// import { FormService } from '../services/form.service';
// import { MatDialogRef } from '@angular/material/dialog';

// import { HttpClientModule } from '@angular/common/http';
// import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar'; // ✅ ייבוא MatSnackBar
// import { MatSelectModule } from '@angular/material/select';  // ✅ הוספת select

// import { ActivatedRoute, Router } from '@angular/router';
// import { HttpClient } from '@angular/common/http';
// import { time } from 'console';

// @Component({
//   selector: 'app-form',
//   standalone: true,
//   imports: [ CommonModule,
//     MatCardModule,
//     MatFormFieldModule,
//     MatInputModule,
//     MatButtonModule,
//     ReactiveFormsModule,
//     HttpClientModule,
//     MatSelectModule,
//     MatSnackBarModule,],
//   templateUrl: './form.component.html',
//   styleUrl: './form.component.css'
// })
// export class FormComponent {

//   patientForm: FormGroup;
//   successMessage: string = '';
//   errorMessage: string = '';
//   showPopup: boolean = false;

//   constructor(private fb: FormBuilder, private formService: FormService,    private snackBar: MatSnackBar // ✅ הזרקת MatSnackBar
//   ) {
//     this.patientForm = this.fb.group({
     
//       status: ['', Validators.required]
//     });
    
//   }

 
  

//   get status(): AbstractControl | null {
//     return this.patientForm.get('status');
//   }
//   get time(): AbstractControl | null {
//     return this.patientForm.get('time');
//   }

//   openPopup() {
//     this.showPopup = true;
//   }

//   closePopup() {
//     this.showPopup = false;
//   }

//   addPatientFile() {
//     if (this.patientForm.invalid) {
//       this.errorMessage = 'נא למלא את כל השדות החיוניים';
//       return;
//     }

//     const { status } = this.patientForm.value;
    
//     this.formService.addPatientFile(status).subscribe({


//   next: (response: any) => {

//     this.snackBar.open(`hi ${response.message}`, 'close', {
//       duration: 3000,
//       panelClass: ['success-snackbar']
//     });
//   },
//   error: () => {
//     this.snackBar.open('error', 'close', {
//       duration: 3000,
//       panelClass: ['error-snackbar']
//     });
  
// }
// });
// }


// }
