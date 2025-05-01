

import { Component, Inject, Optional, PLATFORM_ID, OnInit } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { AllQueueComponent } from '../all-queue/all-queue.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatDialogRef } from '@angular/material/dialog';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AllQueueForPatientsComponent } from '../all-queue-for-patients/all-queue-for-patients.component';
import { ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-queue',
  standalone: true,
  imports: [MatDialogModule, CommonModule, ReactiveFormsModule],
  templateUrl: './queue.component.html',
  styleUrl: './queue.component.css'
})
export class QueueComponent implements OnInit {
  nextPatient: any;
  dialogRef!: MatDialogRef<any>;
  isDialogOpen: boolean = false;
  isAdmin: boolean = false;
  isDoctor: boolean = false;
  isSecretary: boolean = false;
  isPatientEnded: boolean = false;
  avgWaiting: number | null = null;
  queueLength: number | null = null;
  avgTreatmentTime = 10;
  isTreatmentDialogOpen = false;
  currentStep: number = 1; // התחלתי בשלב 1
  private isBrowser: boolean;

  treatmentForm: FormGroup = new FormGroup({
    diagnosis: new FormControl(''),
    procedure: new FormControl(''),
    recommendations: new FormControl('')
  });
  
  constructor(
    private queueService: QueueService, 
    public dialog: MatDialog, 
    private cdr: ChangeDetectorRef, 
    private snackBar: MatSnackBar,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    this.loadStoredPatient();
    this.checkAdmin();
    this.checkDoctor();
    this.checkSecretary();
    
    // אם יש מטופל שלוף מאחסון, נעדכן את שלב הטיפול בהתאם לסטטוס שלו
    if (this.nextPatient) {
      if (this.nextPatient.Status === 'In Treatment') {
        this.currentStep = 2;
      } else if (this.nextPatient.Status === 'Treated') {
        this.currentStep = 4;
      }
    }
  }

  // פונקציה לחישוב רוחב קו ההתקדמות
  getProgressWidth() {
    // For a 4-step process
    const percentage = (this.currentStep - 1) / 3 * 100;
    return `calc(${percentage}% - 80px)`; /* Increased from 40px to 80px to make line shorter */
  }

  // שאר הקוד נשאר אותו דבר...
  openTreatmentDialog() {
    this.isTreatmentDialogOpen = true;
  }

  speak(text: string) {
    if (this.isBrowser && 'speechSynthesis' in window) {
      const speech = new SpeechSynthesisUtterance(text);
      speech.lang = 'en-US';
      speech.rate = 1;
      speech.pitch = 1;
      window.speechSynthesis.speak(speech);
    } else {
      console.warn("דפדפן זה אינו תומך במערכת כריזה");
    }
  }

  fetchNextPatient() {
    this.queueService.getNextPatient().subscribe({
      next: (data) => {
        if (data && data.patient) {  
          this.nextPatient = data;
          this.currentStep = 1; // איפוס השלב בעת קבלת מטופל חדש
          this.cdr.detectChanges();

          this.isPatientEnded = false;
          console.log("מטופל הבא:", this.nextPatient);
          
          const message = `The next patient is ${this.nextPatient.patient.id}, ID number ${this.nextPatient.patient.patientIdentity}. Please enter the room.`;
          this.speak(message);
          this.savePatientToStorage();
        } else {
          console.warn("אין מטופלים זמינים כרגע.");
          this.checkIfQueueIsEmpty();
        }
      },
      error: (err) => {
        console.error("שגיאה בקבלת המטופל הבא", err);
        this.nextPatient = null;
        this.checkIfQueueIsEmpty();
      }
    });
  }

  
  
  startVoiceRecognition() {
    if (this.isBrowser) {
      const recognition = new (window as any).webkitSpeechRecognition();
      recognition.lang = 'en-US';
      recognition.start();
    
      recognition.onresult = (event: any) => {
        const command = event.results[0][0].transcript.toLowerCase();
        if (command.includes("next patient")) {
          this.fetchNextPatient();
        }
      };
    }
  }
    // בלי הקול האוטומטי

  // fetchNextPatient() {
  //   this.queueService.getNextPatient().subscribe({
  //     next: (data) => {
  //       if (data && data.patient) {  // וידוא שהתקבל מטופל תקין
  //         this.nextPatient = data;
  //         this.isPatientEnded = false; 
  //         // יש מטופל, אז אין תור ריק
  //         console.log("מטופל הבא:", this.nextPatient);
  //       } else {
  //         console.warn("אין מטופלים זמינים כרגע.");
  //         this.checkIfQueueIsEmpty(); // בדיקה אם כל התור ריק

  //        // this.nextPatient = null;
  //       }
  //     },
  //     error: (err) => {
  //       console.error("שגיאה בקבלת המטופל הבא", err);
  //       this.nextPatient = null;
  //       this.checkIfQueueIsEmpty();
  //     }
  //   });
  // }
  private checkIfQueueIsEmpty() {
    this.queueService.getQueueSize().subscribe({
      next: (size) => {
        this.isPatientEnded = size === 0;
        this.cdr.detectChanges();
        console.log(`מספר המטופלים בתור: ${size}`);
      },
      error: (err) => {
        console.error("שגיאה בקבלת גודל התור", err);
        this.isPatientEnded = true;
      }
    });
  }
  
  startTreatment(patientId: number) {
    if (this.nextPatient) {
      console.log(`התחלת טיפול למטופל: ${this.nextPatient.id}`);
      this.queueService.startTreatment(patientId).subscribe(() => {
        this.nextPatient.Status = "In Treatment"; 
        this.currentStep = 2; // עדכון לצעד 2 - התחלת טיפול
        this.savePatientToStorage();
        console.log("סטטוס עודכן ל-In Treatment");
        this.cdr.detectChanges(); // לוודא רענון תצוגה
      }, error => {
        console.error("שגיאה בהתחלת טיפול", error);
      });
    }
  }

  endTreatment(patientId: number) {
    console.log("סיום טיפול למטופל:", patientId);
    this.queueService.endTreatment(patientId).subscribe(() => {
      if (this.nextPatient) {
        this.nextPatient.Status = "Treated"; 
        this.currentStep = 4; // עדכון לצעד 4 - סיום טיפול
        this.savePatientToStorage(); // שמירת המצב העדכני לפני מחיקה
        setTimeout(() => {
          this.clearPatientStorage(); // מחיקת המטופל לאחר סיום הטיפול
        }, 3000); // השהייה קלה לאפשר צפייה בשלב האחרון
      }
      console.log("טיפול הסתיים, ממתין ללחיצה על 'Get Next Patient'");
      this.cdr.detectChanges(); // לוודא רענון תצוגה
    }, error => {
      console.error("שגיאה בסיום טיפול", error);
    });
  }
  
  openAllQueueDialog() {
    const dialogRef = this.dialog.open(AllQueueComponent, {
      width: '80%',
      height: '80%',
      disableClose: true 
    });

    dialogRef.afterClosed().subscribe(() => {
      // אין צורך לרענן את התור כי המטופל כבר נשמר בזיכרון
    });
  }

  openAllQueueForPatientDialog() {
    const dialogRef = this.dialog.open(AllQueueForPatientsComponent, {
      width: '80%',
      height: '80%',
      disableClose: true 
    });

    dialogRef.afterClosed().subscribe(() => {
      // אין צורך לרענן את התור כי המטופל כבר נשמר בזיכרון
    });
  }
  
  openQueueDialog() {
    this.isDialogOpen = true;
  }

  checkAdmin() {
    if (this.isBrowser) {
      const role = localStorage.getItem('role');
      this.isAdmin = role === '1';
    }
  }

  checkDoctor() {
    if (this.isBrowser) {
      const role = localStorage.getItem('role');
      this.isDoctor = role === '2';
    }
  }

  checkSecretary() {
    if (this.isBrowser) {
      const role = localStorage.getItem('role');
      this.isSecretary = role === '3';
    }
  }

  private savePatientToStorage() {
    if (this.isBrowser && this.nextPatient) {
      localStorage.setItem('currentPatient', JSON.stringify(this.nextPatient));
    }
  }

  private loadStoredPatient() {
    if (this.isBrowser) {
      const storedPatient = localStorage.getItem('currentPatient');
      if (storedPatient) {
        this.nextPatient = JSON.parse(storedPatient);
        console.log(`מטופל שוחזר מהאחסון: ${this.nextPatient.id}`);
        
        // עדכון מצב הסטפר בהתאם לסטטוס המטופל
        if (this.nextPatient.Status === 'In Treatment') {
          this.currentStep = 2;
        }
      }
    }
  }

  private clearPatientStorage() {
    if (this.isBrowser) {
      localStorage.removeItem('currentPatient');
    }
  }

  sendSummary(patientId: number) {
    const treatmentData = {
      diagnosis: this.treatmentForm.get('diagnosis')?.value, 
      procedure: this.treatmentForm.get('procedure')?.value,
      recommendations: this.treatmentForm.get('recommendations')?.value
    };

    this.queueService.sendTreatmentSummary(patientId, treatmentData).subscribe({
      next: () => {
        this.currentStep = 3; // עדכון לצעד 3 - שליחת סיכום
        this.isTreatmentDialogOpen = false; // סגירת הדיאלוג
        this.savePatientToStorage(); // שמירת המצב העדכני
        
        this.snackBar.open(`The summary sent successfully`, 'close', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.cdr.detectChanges(); // לוודא רענון תצוגה
      },
      error: () => {
        this.snackBar.open('Error', 'close', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }
}