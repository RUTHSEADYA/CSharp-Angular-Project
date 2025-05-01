import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QueueService {
  private baseUrl = 'https://localhost:63026/api/queue';


  constructor(private http: HttpClient) {}

  getNextPatient(): Observable<any> {
    return this.http.get(`${this.baseUrl}/next`);
  }

  addPatient(patient: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/add`, patient);
  }

  startTreatment(patientId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/start-treatment/${patientId}`, {});
  }

  endTreatment(patientId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/end-treatment/${patientId}`, {});
  }
  getAllPatients(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/all-by-score`);
  }
  getQueueSize(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/size`);
  }

  getQueueAvarage(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/avarage`);
  }
  getIdPatientInTreatment():Observable<number>{
    return this.http.get<number>(`${this.baseUrl}/getPatientInTreatment`)
  }

  sendSumarry(patientId:number){
    return this.http.post(`https://localhost:63026/api/Treatment/summaryTreatment/${patientId}`,{})
  }


  sendTreatmentSummary(patientId: number, treatmentData: any): Observable<any> {
    return this.http.post(`https://localhost:63026/api/Treatment/summaryTreatment/${patientId}`, treatmentData);
  }
  
  
}
