
// import { Component, EventEmitter, Output } from '@angular/core';
// import { Router } from '@angular/router';
// import { MatDialog } from '@angular/material/dialog';
// import { LoginComponent } from '../login/login.component';
// import { AuthService } from '../services/auth.service';

// @Component({
//   selector: 'app-nav-bar',
//   standalone: true,
//   imports: [],
//   templateUrl: './nav-bar.component.html',
//   styleUrls: ['./nav-bar.component.css']
// })
// export class NavBarComponent {
//   userRole: number | null = null;
//   showPatientButton: boolean = false;
//   showPatientButtons = false;
//   @Output() togglePatients = new EventEmitter<boolean>(); 
  
//   constructor(
//     private router: Router,
//     private dialog: MatDialog,
//     private authService: AuthService
//   ) {
//     if (typeof localStorage !== 'undefined') {
//       const roleString = localStorage.getItem('role');
//       this.userRole = roleString ? parseInt(roleString, 10) : null;
//       this.showPatientButton = this.userRole === 1 || this.userRole === 3;
//     }
//   }

//    isActiveRoute(route: string): boolean {
//     return this.router.url === route;
//   }
  
//   navigateToPatients() {
//     this.togglePatients.emit(true);
//     this.router.navigate(['/patient']);
//   }
  
//   navigateToLogin() {
//     this.togglePatients.emit(false);
//     this.router.navigate(['/login']);
//   }
  
//   navigateToPatientAttribute() {
//     this.showPatientButton = true;
//     this.router.navigate(['/patient-attribute']);
//   }
  
//   navigateToQueue() {
//     this.togglePatients.emit(false);
//     this.router.navigate(['/queue']);
//   }
  
//   openAuthDialog() {
//     this.dialog.open(LoginComponent, { width: '400px' });
//   }
  
//   isLoggedIn(): boolean {
//     return !!localStorage.getItem('token');
//   }
  


//   logout() {
//     this.authService.logout();
//     localStorage.clear();
    
//     this.userRole = null;
//     this.showPatientButton = false;
//     this.showPatientButtons = false;
    
//     this.router.navigate(['/login']).then(() => {
//       window.location.reload();
//     });
//   }
// }

import { Component, EventEmitter, Output, PLATFORM_ID, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';
import { AuthService } from '../services/auth.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [],
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  userRole: number | null = null;
  showPatientButton: boolean = false;
  @Output() togglePatients = new EventEmitter<boolean>();
  private isBrowser: boolean;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private authService: AuthService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit() {
    this.updateUserRole();
  }

  updateUserRole() {
    if (this.isBrowser) {
      const roleString = localStorage.getItem('role');
      this.userRole = roleString ? parseInt(roleString, 10) : null;
      this.showPatientButton = this.userRole === 1 || this.userRole === 3;
    }
  }

  isActiveRoute(route: string): boolean {
    return this.router.url === route;
  }
  
  navigateToPatients() {
    this.togglePatients.emit(true);
    this.router.navigate(['/patient']);
  }
  
  navigateToLogin() {
    this.togglePatients.emit(false);
    this.router.navigate(['/login']);
  }
  
  navigateToPatientAttribute() {
    this.router.navigate(['/patient-attribute']);
  }
  
  navigateToQueue() {
    this.togglePatients.emit(false);
    this.router.navigate(['/queue']);
  }
  
  openAuthDialog() {
    const dialogRef = this.dialog.open(LoginComponent, { width: '400px' });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateUserRole();
      }
    });
  }
  
  isLoggedIn(): boolean {
    if (this.isBrowser) {
      return !!localStorage.getItem('token');
    }
    return false;
  }
  
  logout() {
    this.authService.logout();
    if (this.isBrowser) {
      localStorage.clear();
    }
    this.userRole = null;
    this.showPatientButton = false;
    this.router.navigate(['/login']).then(() => {
      window.location.reload();
    });
  }
}
