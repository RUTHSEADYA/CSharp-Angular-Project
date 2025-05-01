import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DatePipe, Time } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private baseUrl = 'https://localhost:63026/api/';


  constructor(private http: HttpClient) { }


  addPatient(patientData: { patientIdentity: string, name: string; gender: string, dateOfBirth: DatePipe, address: string; phone: string; files: { status: string }[] }): Observable<any> {
    return this.http.post(`${this.baseUrl}patient/addPatient`, patientData);
  }


  addPatientFile(
    patientId: number,
    fileData: { status: string, descreption: string, patientAttributes?: { id: number, attributeName?: string, score?: number }[] }
  ): Observable<any> {

    return this.http.post(`${this.baseUrl}patient/addPatientFile/${patientId}`, {
      patientId,
      status: fileData.status,
      descreption: fileData.descreption,
      patientAttributes: (fileData.patientAttributes || []).map(attr => ({
        id: attr.id,
        attributeName: attr.attributeName || '',
        score: attr.score || 0
      }))
    });
  }


  getPatientById(patientId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}patient/getPatient/${patientId}`);
  }
  getAttributes(): Observable<any> {
    return this.http.get(`${this.baseUrl}patientFile/attributes`);
  }



  calculateScore(patientFile: any): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}patientFile/calculateScore`, patientFile);
  }

  getPatientFileById(patientFileId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}patientFile/getPatientFileById/${patientFileId}`);
  }


  updateFinalScore(patientFileId: number, finalScore: number): Observable<any> {
    const body = { finalScore: finalScore };
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.put<any>(`${this.baseUrl}patientFile/updateFinalScore/${patientFileId}`, body, { headers });
  }
  deletePatientAttribute(clientAttributeId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}clientAttribute/deleteClientAttribute/${clientAttributeId}`);
  }


  getPatientByIdentity(identity: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}patient/searchByIdentity/${identity}`);
  }

  getPatients(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}patient/getAllPatints`);
  }

  deletePatientByNumberId(patientId: string) {
    return this.http.delete(`${this.baseUrl}patient/deleteByNumberId/${patientId}`);
  }
  deletePatient(patientId: number) {
    return this.http.delete(`${this.baseUrl}patient/delete/${patientId}`);
  }
  updatePatient(patientId: number, patient: any): Observable<any> {
    return this.http.put(`${this.baseUrl}patient/updatePatient/${patientId}`, patient)
  }


}
