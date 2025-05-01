import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class FormService {
  private baseUrl ='https://localhost:63026/api/patientFile';// כתובת ה-API של השרת

  constructor(private http: HttpClient) { }

     
  
       // פונקציה להרשמה
       addPatientFile(status:string): Observable<any> {
        return this.http.post(`${this.baseUrl}/addPatientFile`, { status});
      }
}
