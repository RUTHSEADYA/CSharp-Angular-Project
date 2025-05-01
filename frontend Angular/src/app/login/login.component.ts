
// import { Component, Inject } from '@angular/core';
// import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
// import { MatCardModule } from '@angular/material/card';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatInputModule } from '@angular/material/input';
// import { MatButtonModule } from '@angular/material/button';
// import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';
// import { AuthService } from '../services/auth.service';
// import { HttpClientModule } from '@angular/common/http';
// import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
// import { MatSelectModule } from '@angular/material/select';
// import { Router } from '@angular/router';
// import { ChangeDetectorRef } from '@angular/core';

// @Component({
//   selector: 'app-login',
//   templateUrl: './login.component.html',
//   styleUrls: ['./login.component.css'],
//   standalone: true,
//   imports: [
//     CommonModule,
//     MatCardModule,
//     MatFormFieldModule,
//     MatInputModule,
//     MatButtonModule,
//     ReactiveFormsModule,
//     HttpClientModule,
//     MatSelectModule,
//     MatSnackBarModule,
//     MatDialogModule 
//   ]
// })
// export class LoginComponent {
//   loginForm: FormGroup;
//   registerForm: FormGroup;
//   errorMessage: string | null = null;
//   registerError: string | null = null;
//   showRegister: boolean = false;

//   constructor(
//     private authService: AuthService,
//     private router: Router,
//     private snackBar: MatSnackBar,   
//     private cdr: ChangeDetectorRef, 

//     @Inject(MAT_DIALOG_DATA) public data: any 
//   ) {
//     this.loginForm = new FormGroup({
//       username: new FormControl('', Validators.required),
//       password: new FormControl('', Validators.required)
//     });

//     this.registerForm = new FormGroup({
//       username: new FormControl('', Validators.required),
//       password: new FormControl('', Validators.required),
//       role: new FormControl('', Validators.required)
//     });
//   }

//   login() {
//     if (this.loginForm.valid) {
//       const { username, password } = this.loginForm.value;
//       this.authService.login(username, password).subscribe({
//         next: (response: any) => {
//           if (response.token) {
//             localStorage.setItem('token', response.token); 
//             localStorage.setItem('role', response.role.toString()); 
            
//             this.snackBar.open(`Hi ${username}, ${response.message}`, 'close', { duration: 3000 })
//             this.router.navigate(['/navbar']);
//             window.location.reload();
  
//           } else {
//             this.snackBar.open('התחברות נכשלה, נסה שוב!', 'close', { duration: 3000 });
//           }
//         },
//         error: () => {
//           this.snackBar.open('שם משתמש או סיסמה שגויים!', 'close', { duration: 3000 });
//         }
//       });
//     }
//   }
  

//   register() {
//     if (this.registerForm.valid) {
//       const { username, password, role } = this.registerForm.value;
//       const roleValue = parseInt(role, 10);
//       this.authService.signUp(username, password, roleValue).subscribe({
//         next: () => {
//           this.snackBar.open('נרשמת בהצלחה! כעת ניתן להתחבר.', 'סגור', { duration: 3000 });
//           this.showRegister = false;
//         },
//         error: (err) => {
//           const errorMessage = err.error || 'שגיאה בהרשמה';
//           this.snackBar.open(errorMessage, 'סגור', { duration: 3000 });
//         }
//       });
//     }
//   }

  

//   toggleRegister() {
//     this.showRegister = !this.showRegister;
//     this.errorMessage = null;
//     this.registerError = null;
//   }

//   closeDialog() {
//   //  this.dialogRef.close(); // ✅ סגירת הדיאלוג
//   }
// }
import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSelectModule,
    MatSnackBarModule,
    MatDialogModule
  ]
})
export class LoginComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  errorMessage: string | null = null;
  registerError: string | null = null;
  showRegister: boolean = false;
  private isBrowser: boolean;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar,
    @Inject(PLATFORM_ID) platformId: Object,
    public dialogRef?: MatDialogRef<LoginComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any = null
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    
    this.loginForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });

    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      role: new FormControl('', Validators.required)
    });
  }

  login() {
    if (this.loginForm.valid) {
      const { username, password } = this.loginForm.value;
      this.authService.login(username, password).subscribe({
        next: (response: any) => {
          if (response.token) {
            if (this.isBrowser) {
              localStorage.setItem('token', response.token); 
              localStorage.setItem('role', response.role.toString()); 
            }
            
            this.snackBar.open(`Hi ${username}, ${response.message}`, 'close', { duration: 3000 });
            
            // אם הקומפוננטה פתוחה כדיאלוג, סגור אותה
            if (this.dialogRef) {
              this.dialogRef.close(true);
            }
            
            this.router.navigate(['/navbar']).then(() => {
              // הימנע מטעינה מחדש של העמוד שעלולה לגרום להופעת הקומפוננטה פעמיים
            });
          } else {
            this.snackBar.open('התחברות נכשלה, נסה שוב!', 'close', { duration: 3000 });
          }
        },
        error: () => {
          this.snackBar.open('שם משתמש או סיסמה שגויים!', 'close', { duration: 3000 });
        }
      });
    }
  }
  
  register() {
    if (this.registerForm.valid) {
      const { username, password, role } = this.registerForm.value;
      const roleValue = parseInt(role, 10);
      this.authService.signUp(username, password, roleValue).subscribe({
        next: () => {
          this.snackBar.open('נרשמת בהצלחה! כעת ניתן להתחבר.', 'סגור', { duration: 3000 });
          this.showRegister = false;
        },
        error: (err) => {
          const errorMessage = err.error || 'שגיאה בהרשמה';
          this.snackBar.open(errorMessage, 'סגור', { duration: 3000 });
        }
      });
    }
  }

  toggleRegister() {
    this.showRegister = !this.showRegister;
    this.errorMessage = null;
    this.registerError = null;
  }
  
  closeDialog() {
    // בדוק אם הקומפוננטה פתוחה כדיאלוג
    if (this.dialogRef) {
      this.dialogRef.close();
    } else {
      // אם לא, נווט לעמוד אחר
      this.router.navigate(['/navbar']);
    }
  }
}