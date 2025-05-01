import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {  Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PatientAttributesService {


  private baseUrl ='https://localhost:63026/api/';


  constructor(private http: HttpClient) {}

      addPatientAttribute(patientAttributeData: { attributeName: string; score: number; }): Observable<any> {
        return this.http.post(`${this.baseUrl}clientAttribute/addClientAttribute`, patientAttributeData);
      }
      deletePatientAttribute(clientAttributeId:number): Observable<any> {
        return this.http.delete(`${this.baseUrl}clientAttribute/deleteClientAttribute/${clientAttributeId}`);
      }

      getAttributes(): Observable<any> {
        return this.http.get(`${this.baseUrl}patientFile/attributes`);
      }
      
    }

