import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router'; // ✅ ייבוא ה-Router

import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:63026/api/auth';// כתובת ה-API של השרת

  constructor(private http: HttpClient, private router: Router) {

  }

  signUp(username: string, password: string, role: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/signUp`, { username, password, role });
  }


  login(username: string, password: string) {
    console.log('🔹 Sending Login Request:', { username, password });
    return this.http.post<{ token: string; role: number }>(
      `${this.baseUrl}/login`,
      { username, password }
    ).pipe(
      tap(response => {
        console.log('🔹 Server Response:', response);
        if (response.token) {
          localStorage.setItem('token', response.token);
          console.log("✅ Token Saved in LocalStorage:", localStorage.getItem('token')); // בדיקה שהטוקן נשמר

          localStorage.setItem('role', response.role.toString());
          console.log('🔹 User Role:', localStorage.getItem('role'));

        }
      })
    );
  }

  logout() {
    console.log("🔹 Logging out...");
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    console.log("✅ User logged out successfully.");

    // ניתן להפנות את המשתמש לדף ההתחברות
    this.router.navigate(['/login']);
  }

}
