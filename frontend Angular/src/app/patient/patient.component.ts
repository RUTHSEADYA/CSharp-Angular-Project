

import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PatientService } from '../services/patient.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import { ScorePipe } from './score.pipe'; // import את ה-Pipe


@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSelectModule
  ],
  templateUrl: './patient.component.html',
  styleUrls: ['./patient.component.css'],
  providers: [ScorePipe] // Add ScorePipe here

})
export class PatientComponent implements OnInit {
  @Input() showButtons = false;

  selectedPatientId: number | null = null;
  isAdmin: boolean = false;
  isDoctor: boolean = false;
  isSecretary: boolean = false;
  addPatientForm: FormGroup;
  updatePatientForm: FormGroup;
  patientForm: FormGroup;
  successMessage: string = '';
  errorMessage: string | null = null;
  showPopup: boolean = false;
  showUpdatePopup: boolean = false;
  showSecondPopup: boolean = false;
  showSearchPopup: boolean = false;
  attributes: any[] = []; 
  selectedAttributes: number[] = [];
  totalScore: number = 0;
  public lastFileId: number | null = null;
  searchForm: FormGroup;
  selectedPatient: any;
  patientFiles: any[] = [];

  constructor(private fb: FormBuilder, private patientService: PatientService, private snackBar: MatSnackBar, private cdr: ChangeDetectorRef, private scorePipe: ScorePipe) {
      this.addPatientForm = this.fb.group({
      patientIdentity: ['', [Validators.required, Validators.pattern(/^\d{9}$/), this.israeliIdValidator]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\u0590-\u05FF\s]+$/)]],
      gender: ['', Validators.required],
      dateOfBirth: ['', [Validators.required, this.dateOfBirthValidator]],
      address: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      phone: ['', [Validators.required, Validators.pattern(/^05\d{8}$/)]],
      email: ['', [Validators.required, Validators.email, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]],
      files: this.fb.array([])
    });

    this.patientForm = this.fb.group({
      status: ['', Validators.required],
      descreption: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      attributes: this.fb.array([]),// הוספת מאפיינים כ-FormArray
      finalScore: [0, Validators.required] // ודא שהוא מאותחל עם ערך כלשהו
    });

    this.searchForm = this.fb.group({
      identity: ['', [Validators.required, Validators.pattern(/^\d{9}$/), this.israeliIdValidator]]
    });

    this.updatePatientForm = this.fb.group({
      patientIdentity: ['', [Validators.required, Validators.pattern(/^\d{9}$/), this.israeliIdValidator]],
      name: [this.name, [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\u0590-\u05FF\s]+$/)]],
      gender: [this.gender, Validators.required],
      dateOfBirth: [this.dateOfBirth, [Validators.required, this.dateOfBirthValidator]],
      address: [this.address, [Validators.required, Validators.minLength(5), Validators.maxLength(100)]],
      phone: [this.phone, [Validators.required, Validators.pattern(/^05\d{8}$/)]],
      email: [this.email, [Validators.required, Validators.email, Validators.pattern(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)]],
      attributes: this.fb.array([]), // מאפיינים נבחרים
      files: this.fb.array([])
    })
  }


  israeliIdValidator(control: AbstractControl): { [key: string]: any } | null {
    const id = control.value;
    if (!id || typeof id !== 'string') {
      return null;
    }

    if (id.length !== 9 || !/^\d+$/.test(id)) {
      return { 'invalidIsraeliId': true };
    }

    let sum = 0;
    for (let i = 0; i < 9; i++) {
      let digit = parseInt(id.charAt(i), 10);
      if (i % 2 === 0) {
        digit *= 1;
      } else {
        digit *= 2;
        if (digit > 9) {
          digit = digit % 10 + Math.floor(digit / 10);
        }
      }
      sum += digit;
    }

    return sum % 10 === 0 ? null : { 'invalidIsraeliId': true };
  }

  dateOfBirthValidator(control: AbstractControl): { [key: string]: any } | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();

    if (selectedDate > currentDate) {
      return { 'futureDate': true };
    }

    // Check if the patient is not older than 120 years
    const minDate = new Date();
    minDate.setFullYear(currentDate.getFullYear() - 120);

    if (selectedDate < minDate) {
      return { 'tooOld': true };
    }

    return null;
  }


  ngOnInit() {
    this.checkAdmin();
    this.checkDoctor();
    this.checkSecretary();
    this.getAttributes();

    if (this.selectedPatientId) {
      this.patientService.getPatientFileById(this.selectedPatientId).subscribe(file => {
        this.patientForm.patchValue({
          id: file.id,
          status: file.status,
          descreption: file.descreption,
          attributes: file.patientAttributes
        });
        this.lastFileId = file.id;
        this.cdr.detectChanges(); 
        console.log("🔍 lastFileId עודכן:", this.lastFileId);
      });
    } else {
      console.warn('⚠️ selectedPatientId is null or undefined. Waiting for selection.');
    }
  }



  selectPatient(patientId: number) {
    this.selectedPatientId = patientId;
    console.log('Selected patient set to:', this.selectedPatientId); // בדיקה
  }

  get patientIdentity(): AbstractControl | null { return this.addPatientForm.get('patientIdentity'); }
  get name(): AbstractControl | null { return this.addPatientForm.get('name'); }
  get gender(): AbstractControl | null { return this.addPatientForm.get('gender'); }
  get dateOfBirth(): AbstractControl | null { return this.addPatientForm.get('dateOfBirth'); }
  get address(): AbstractControl | null { return this.addPatientForm.get('address'); }
  get phone(): AbstractControl | null { return this.addPatientForm.get('phone'); }
  get email(): AbstractControl | null { return this.addPatientForm.get('email'); }
  get files(): FormArray { return this.addPatientForm.get('files') as FormArray; }

  openPopup() { this.showPopup = true; }
  closePopup() { this.showPopup = false; }

  openSecondPopup() { this.showSecondPopup = true; }
  closeSecondPopup() { this.showSecondPopup = false; }
  
  openSearchPopup() {this.showSearchPopup = true;} 
  closeSearchPopup() { this.showSearchPopup = false; }

  addFile() {
    this.files.push(this.fb.group({
      status: ['', Validators.required],
      descreption: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]]
    }));
  }

  removeFile(index: number) {
    this.files.removeAt(index);
  }

  getAttributes() {
    this.patientService.getAttributes().subscribe({
      next: (data: any) => {
        console.log('Attributes received:', data);
        this.attributes = data;
      },
      error: (err) => {
        console.error('Error fetching attributes:', err);
      }
    });
  }

 
  openUpdatePopup(patient: any) {

    this.showUpdatePopup = true;
    this.updatePatientForm.patchValue({
      patientIdentity: patient.patientIdentity,
      name: patient.name,
      gender: patient.gender,
      dateOfBirth: this.formatDateForInput(patient.dateOfBirth), // המרת התאריך לפורמט המתאים לשדה מסוג date
      address: patient.address,
      phone: patient.phone,
      email: patient.email
    });

    this.selectedPatientId = patient.id;
  }

  formatDateForInput(dateString: string): string {
    if (!dateString) return '';

    const date = new Date(dateString);
    return date.toISOString().split('T')[0]; 
  }

  closeUpdatePopup() {
    this.showUpdatePopup = false;
  }

  updatePatient() {
    if (this.updatePatientForm.invalid) {
      this.updatePatientForm.markAllAsTouched();
      this.errorMessage = 'נא למלא את כל השדות החיוניים';
      return;
    }

    if (!this.selectedPatientId) {
      this.errorMessage = 'לא נבחר מטופל לעדכון';
      return;
    }

    const patientData = this.updatePatientForm.value;
    console.log('Sending updated patient data:', patientData);

    this.patientService.updatePatient(this.selectedPatientId, patientData).subscribe({
      next: (response: any) => {
        this.snackBar.open('מטופל עודכן בהצלחה!', 'סגור', { duration: 3000, panelClass: ['success-snackbar'] });
        this.successMessage = 'מטופל עודכן בהצלחה!';
        this.errorMessage = '';

        // עדכון הנתונים המוצגים (אם המטופל המעודכן הוא המטופל הנבחר)
        if (this.selectedPatient && this.selectedPatient.id === this.selectedPatientId) {
          this.selectedPatient = { ...this.selectedPatient, ...patientData };
        }

        // סגירת החלון
        this.closeUpdatePopup();

        // רענון הנתונים - אופציונלי, תלוי בלוגיקה של האפליקציה
        // this.searchPatientById(); // אם זו פונקציה שמרעננת את נתוני המטופל הנוכחי
      },
      error: (err) => {
        console.error('Error updating patient:', err);
        const errorMessage = err.error?.message || 'שגיאה בעדכון המטופל';
        this.snackBar.open(errorMessage, 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
        this.errorMessage = errorMessage;
      }
    });
  }

  // addAttribute(attribute?: any) {
  //   const attributesArray = this.patientForm.get('attributes') as FormArray;
  //   const attributeGroup = this.fb.group({
  //     id: [attribute?.id ?? 0, Validators.required], 
  //     attributeName: [attribute?.attributeName ?? 'לא מוגדר', [Validators.required, Validators.minLength(1)]],
  //     score: [attribute?.score ?? 0, [Validators.required, Validators.min(0), Validators.max(100)]]
  //   });

  //   attributesArray.push(attributeGroup);
  // }


  toggleAttribute(attribute: any, event: any) {
    let attributesArray = this.patientForm.get('attributes') as FormArray;

    // ודא שקיים מערך attributesכ
    if (!attributesArray) {
      this.patientForm.setControl('attributes', this.fb.array([]));
      attributesArray = this.patientForm.get('attributes') as FormArray;
    }

    console.log('🔹 Attribute Clicked:', attribute); // בדיקה: מה מתקבל מהלחיצה?

    if (event.target.checked) {
      console.log('✅ Adding attribute:', attribute);

      // בדוק אם המאפיין כבר קיים
      const exists = attributesArray.controls.some(control => control.value.id === attribute.id);
      if (exists) {
        console.log('⚠️ Attribute already exists, skipping:', attribute);
        return;
      }

      // יצירת אובייקט מאפיין עם הנתונים האמיתיים
      const newAttribute = this.fb.group({
        id: [attribute.id, Validators.required],
        attributeName: [attribute.attributeName, [Validators.required, Validators.minLength(1)]],
        score: [attribute.score, [Validators.required, Validators.min(0), Validators.max(100)]]
      });

      attributesArray.push(newAttribute);
    } else {
      // מחיקת המאפיין אם הסימון הוסר
      const index = attributesArray.controls.findIndex(control => control.value.id === attribute.id);
      if (index !== -1) {
        console.log('🗑 Removing attribute:', attribute);
        attributesArray.removeAt(index);
      }
    }

    console.log('📌 Selected attributes after change:', attributesArray.value); // הדפסת הנתונים המעודכנים
  }


  addPatient() {
    if (this.addPatientForm.invalid) {
      this.addPatientForm.markAllAsTouched();
      this.errorMessage = 'נא למלא את כל השדות החיוניים';
      return;
    }

    const patientData = this.addPatientForm.value;
    console.log('Sending patient data:', patientData);

    this.patientService.addPatient(patientData).subscribe({
      next: (response: any) => {
        if (response && response.patientId) {
          this.selectedPatientId = response.patientId;
        }

        this.snackBar.open(`patient added successfuly`, 'close', { duration: 3000, panelClass: ['success-snackbar'] });
        this.successMessage = 'מטופל נוסף בהצלחה!';
        this.errorMessage = '';

        this.addPatientForm.reset();
        this.addPatientForm.setControl('files', this.fb.array([]));
        this.addPatientForm.setControl('attributes', this.fb.array([]));
        this.closePopup();
        this.openSecondPopup();
      },
      error: (err) => {
        console.error(err);
        // this.snackBar.open('שגיאה בהוספת מטופל', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
        // this.errorMessage = 'שגיאה בהוספת מטופל';
        const errorMessage = err.error.message || 'error in adding patient';
        this.snackBar.open(errorMessage, 'close', { duration: 3000, panelClass: ['error-snackbar'] })

      }
    });
  }



  // פונקציה לחישוב הניקוד ושמירה
  calculateAndSaveScore() {
    const attributesArray = this.patientForm.get('attributes') as FormArray;

    if (!attributesArray || attributesArray.length === 0) {
      console.warn('⚠️ אין מאפיינים מסומנים.');
      this.totalScore = 0;
      this.patientForm.patchValue({ finalScore: 0 });
      return;
    }

    // חישוב סכום הניקוד באמצעות ה-Pipe
    const total = this.scorePipe.transform(attributesArray.controls.map(control => control.value));

    // עדכון הניקוד בטופס
    this.totalScore = total;
    this.patientForm.patchValue({ finalScore: total });

    console.log('✅ ניקוד כולל מחושב:', total);
    console.log('📝 finalScore בטופס:', this.patientForm.value.finalScore);
  }

  // הוספת טופס למטופל
  addPatientFile() {
    console.log('addPatientFile function started');
    console.log('Patient Form Value:', this.patientForm.value);
    console.log('Form Validity:', this.patientForm.valid);
    console.log('Selected Patient ID:', this.selectedPatientId);

    if (this.patientForm.invalid || !this.selectedPatientId) {
      this.patientForm.markAllAsTouched();
      this.errorMessage = 'שגיאה: לא נמצא מטופל להוספת טופס';
      return;
    }

    const attributesArray = this.patientForm.get('attributes') as FormArray;
    console.log('Form Value:', this.patientForm.value);
    console.log('FormArray Controls:', (this.patientForm.get('attributes') as FormArray).controls);

    const fileData = {
      status: this.patientForm.value.status,
      descreption: this.patientForm.value.descreption,
      finalScore: this.patientForm.value.finalScore ?? 0, // אם null, אתחל ל-0

      patientAttributes: attributesArray.controls.map(control => {
        const attr = control.value;
        return {
          id: attr?.id ?? 0,
          attributeName: attr?.attributeName ?? 'לא מוגדר',
          score: attr?.score ?? 0
        };
      })
    };

    console.log('Final fileData to send:', fileData);

    this.patientService.addPatientFile(this.selectedPatientId, fileData).subscribe({
      next: (response) => {
        console.log('File added successfully:', response);
        console.log('the messega', response.newFileId);

        if (response && response.newFileId) { // בדיקת קיום fileId
          this.lastFileId = response.newFileId;
          console.log("🔍 lastFileId עודכן בהצלחה:", this.lastFileId);
        } else {
          console.warn("⚠️ fileId לא נמצא בתשובת השרת.");
        }

        if (response?.newFileId) { // שמירת ה-ID של הטופס שנוסף
          this.lastFileId = response.newFileId;
          this.cdr.detectChanges();
        }
        this.snackBar.open(`הטופס נוסף בהצלחה!`, 'סגור', { duration: 3000, panelClass: ['success-snackbar'] });
        this.successMessage = 'טופס נוסף בהצלחה!';
        this.errorMessage = '';
        this.patientForm.reset();
        this.closeSecondPopup();
        this.getAttributes();
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('שגיאה בהוספת טופס', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
        this.errorMessage = 'שגיאה בהוספת טופס';
      }
    });
  }

  // עדכון טופס למטופל
  updatePatientFile(fileId: number, finalScore: number): void {
    this.patientService.updateFinalScore(fileId, finalScore).subscribe({
      next: () => {
        console.log('✅ ניקוד עודכן בהצלחה!');
        this.snackBar.open('score updated successfuly', 'close', { duration: 3000, panelClass: ['success-snackbar'] });

        if (this.selectedPatientId) {
          console.log(`🔄 מבצע חיפוש מחדש עבור מטופל ID: ${this.selectedPatientId}`);
          this.searchPatientById();
        } else {
          console.error('❌ שגיאה: אין ID של מטופל נבחר');
        }
      },
      error: err => {
        console.error('❌ שגיאה בעדכון הניקוד:', err);
        this.snackBar.open('שגיאה בעדכון הניקוד', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
      }
    });
  }

  // פונקציה לחישוב חבילות של מאפיינים
  get attributesChunks(): any[][] {
    const chunkSize = 10;
    const chunks = [];
    for (let i = 0; i < this.attributes.length; i += chunkSize) {
      chunks.push(this.attributes.slice(i, i + chunkSize));
    }
    return chunks;
  }


  checkAdmin() {
    const role = localStorage.getItem('role');
    this.isAdmin = role === '1'; // בדיקה אם המשתמש מנהל
  }

  checkDoctor() {
    const role = localStorage.getItem('role');
    this.isDoctor = role === '2'; // בדיקה אם המשתמש מנהל
  }

  checkSecretary() {
    const role = localStorage.getItem('role');
    this.isSecretary = role === '3'; // בדיקה אם המשתמש מנהל
  }

  confirmDelete(clientAttributeId: number) {
    const confirmDelete = confirm("האם אתה בטוח שברצונך למחוק את המאפיין?");
    if (confirmDelete) {
      this.deletePatientAttribute(clientAttributeId);
    }
  }


  deletePatientAttribute(clientAttributeId: number) {
    this.patientService.deletePatientAttribute(clientAttributeId).subscribe({
      next: () => {
        console.log('Attribute deleted successfully');

        // עדכון הרשימה - מסירים את המאפיין שנמחק
        this.attributes = this.attributes.filter(attr => attr.id !== clientAttributeId);

        // אין צורך לעדכן attributesChunks כי הוא מחושב מחדש דרך getter
        this.snackBar.open('המאפיין נמחק בהצלחה!', 'סגור', { duration: 3000, panelClass: ['success-snackbar'] });
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('שגיאה במחיקת המאפיין', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
      }
    });
  }


  confirmDeletePaient(patientId: string) {
    const confirmDelete = confirm("האם אתה בטוח שברצונך למחוק את המאפיין?");
    if (confirmDelete) {
      this.deletePatient(patientId);
    }
  }


  deletePatient(patientId: string) {
    this.patientService.deletePatientByNumberId(patientId).subscribe({
      next: () => {
        console.log('patient deleted successfully');

        this.selectedPatient = false;
        // אין צורך לעדכן attributesChunks כי הוא מחושב מחדש דרך getter
        this.snackBar.open('המטופל נמחק בהצלחה!', 'סגור', { duration: 3000, panelClass: ['success-snackbar'] });
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('שגיאה במחיקת מטופל', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
      }
    });
  }
  searchPatientById() {
    console.log('🔍 חיפוש התחיל...'); // בדיקה אם הפונקציה מופעלת בכלל
    const identity = this.searchForm.get('identity')?.value;
    console.log('🔢 תעודת זהות שהוזנה:', identity); // הדפסה לבדיקה
    if (!identity) {
      console.log('⚠️ אין תעודת זהות');
      return;
    }

    this.patientService.getPatientByIdentity(identity).subscribe(response => {
      console.log('✅ תוצאה מהשרת:', response);
      console.log('📄 טפסים שהתקבלו:', response.Files);
      if (response) {
        this.selectedPatient = {
          patientIdentity: response.patientIdentity,
          id: response.patientId,
          name: response.name,
          gender: response.gender,
          dateOfBirth: response.dateOfBirth,
          address: response.address,
          phone: response.phone,
          email: response.email,
          status: response.status,
          entryTime: response.entryTime
        };
        this.selectedPatientId = response.patientId;
        console.log('🆔 ID שנשמר:', this.selectedPatientId);

        console.log('📝 selectedPatient:', this.selectedPatient); // בדיקה אם הנתונים נשמרים נכון

        this.patientFiles = response.files || [];
        console.log('📂 טפסים שנשמרו:', this.patientFiles);

        this.showSearchPopup = true; // ✅ הצגת הפופ-אפ לאחר שמירת הנתונים



      } else {
        this.selectedPatient = null;
        this.patientFiles = [];
        this.showSearchPopup = false; // ✅ לוודא שהפופ-אפ לא מוצג במקרה של חיפוש ריק
        console.log('❌ מטופל לא נמצא');
      }
    }, error => {
      console.error('❌ שגיאה בעת חיפוש מטופל:', error);
      this.snackBar.open('מטופל לא קיים במערכת ', 'סגור', { duration: 5000, panelClass: ['error-snackbar'] });
      this.selectedPatient = null;
      this.patientFiles = [];
      this.showSearchPopup = false; // ✅ הפופ-אפ לא יוצג במקרה של שגיאה
    });

  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); // מחזיר true אם יש טוקן
  }

  closeAddAnotherForm() {
    this.selectedPatient = false;
  }
}