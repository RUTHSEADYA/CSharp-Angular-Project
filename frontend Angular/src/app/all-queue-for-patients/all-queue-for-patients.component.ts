// import { Component, OnInit } from '@angular/core';
// import { QueueService } from '../services/queue.service';
// import { Subscription, interval } from 'rxjs';
// import { MatDialogRef } from '@angular/material/dialog';
// import { MatIconModule } from '@angular/material/icon'; // ייבוא המודול
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-all-queue-for-patients',
//   standalone: true,
//   imports: [MatIconModule,CommonModule],
//   templateUrl: './all-queue-for-patients.component.html',
//   styleUrl: './all-queue-for-patients.component.css'
// })
// export class AllQueueForPatientsComponent {
//   patients: any[] = [];
//   private refreshSubscription!: Subscription;
//  numberPatient:number|null=null;
//  numberPatientNext:number|null=null;
//  patientNext:any|null=null;



//   constructor(private queueService: QueueService,private dialogRef: MatDialogRef<AllQueueForPatientsComponent>) {}

//   ngOnInit(): void {
//     this.loadQueue();
//       // ריענון הנתונים כל 5 שניות
//       this.refreshSubscription = interval(5000).subscribe(() => {
//         this.loadQueue();
//       });
//       this.numberOfPatientInTreatment();
//       this.numberOfPatientNextInTreatment();
    
//   }

//   loadQueue() {
//     this.queueService.getAllPatients().subscribe(
//       (data) => {
//         this.patients = data.sort((a: any, b: any) => b.finalScore - a.finalScore); // מיון לפי ניקוד
//       },
//       (error) => {
//         this.patients = [];
//       }
//     );
//   }
//   closeDialog() {
//     this.dialogRef.close();
//   }

//   ngOnDestroy(): void {
//     if (this.refreshSubscription) {
//       this.refreshSubscription.unsubscribe(); // מניעת זליגת זיכרון
//     }
//   }

//   numberOfPatientInTreatment(){
//     this.queueService.getIdPatientInTreatment().subscribe({
//       next:(response)=>{
//      this.numberPatient=response;
//      console.log("the number of patient that un treatment ",response)
//       },
//       error:(err)=>{
//         console.log("error to get that number",err)
//       }
//     })
//   }
//     numberOfPatientNextInTreatment(){
//       this.queueService.getNextPatient().subscribe({
//         next:(response)=>{
//        this.numberPatientNext=response.patientId;
//        this.patientNext=response;
//        console.log("the number of patient that un treatment ",response.patientId)
//         },
//         error:(err)=>{
//           console.log("error to get that number",err)
//         }
//       })
//   }

  
  

// }


import { Component, OnInit, Inject, Optional } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { Subscription, interval } from 'rxjs';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-all-queue-for-patients',
  standalone: true,
  imports: [
    MatIconModule,
    CommonModule,
    MatDialogModule
  ],
  templateUrl: './all-queue-for-patients.component.html',
  styleUrl: './all-queue-for-patients.component.css'
})
export class AllQueueForPatientsComponent implements OnInit {
  patients: any[] = [];
  private refreshSubscription!: Subscription;
  numberPatient: number | null = null;
  numberPatientNext: number | null = null;
  patientNext: any | null = null;

  constructor(
    private queueService: QueueService,
    @Optional() private dialogRef: MatDialogRef<AllQueueForPatientsComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.loadQueue();
    // ריענון הנתונים כל 5 שניות
    this.refreshSubscription = interval(5000).subscribe(() => {
      this.loadQueue();
    });
    this.numberOfPatientInTreatment();
    this.numberOfPatientNextInTreatment();
  }

  loadQueue() {
    this.queueService.getAllPatients().subscribe(
      (data) => {
        this.patients = data.sort((a: any, b: any) => b.finalScore - a.finalScore); // מיון לפי ניקוד
      },
      (error) => {
        this.patients = [];
      }
    );
  }
  
  closeDialog() {
    if (this.dialogRef) {
      this.dialogRef.close();
    }
  }

  ngOnDestroy(): void {
    if (this.refreshSubscription) {
      this.refreshSubscription.unsubscribe(); // מניעת זליגת זיכרון
    }
  }

  numberOfPatientInTreatment() {
    this.queueService.getIdPatientInTreatment().subscribe({
      next: (response) => {
        this.numberPatient = response;
        console.log("the number of patient that un treatment ", response);
      },
      error: (err) => {
        console.log("error to get that number", err);
      }
    });
  }
  
  numberOfPatientNextInTreatment() {
    this.queueService.getNextPatient().subscribe({
      next: (response) => {
        this.numberPatientNext = response.patientId;
        this.patientNext = response;
        console.log("the number of patient that un treatment ", response.patientId);
      },
      error: (err) => {
        console.log("error to get that number", err);
      }
    });
  }
}