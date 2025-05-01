import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PatientAttributesService } from '../services/patient-attributes.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-patient-attributes',
  standalone: true,
  imports: [ CommonModule,
      MatCardModule,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule,
      ReactiveFormsModule,
      HttpClientModule,
      MatSelectModule,
      
      ],
  templateUrl: './patient-attributes.component.html',
  styleUrl: './patient-attributes.component.css'
})
export class PatientAttributesComponent {
    addAttributeForm: FormGroup;
    showPopup: boolean = false;
    showAttributesPopup: boolean = false;

    getAttributtesForm:FormGroup;
    attributes: any[] = [];
    

  
    
      constructor(private fb: FormBuilder, private patientAttributeService: PatientAttributesService, private snackBar: MatSnackBar,) {
        this.addAttributeForm = this.fb.group({
          attributeName: ['', Validators.required],
          score: ['', Validators.required],
          
        });
        this.getAttributtesForm=this.fb.group({
          attributes: this.fb.array([]) ,

        })

      }

  get attributeName(): AbstractControl | null { return this.addAttributeForm.get('attributeName'); }
  get score(): AbstractControl | null { return this.addAttributeForm.get('score'); }
  
  openPopup() { this.showPopup = true; }
  closePopup() { this.showPopup = false; }
  openAttributesPopup() {
    this.getAttributes(); // הבאת הנתונים מהשרת לפני הצגת הפופ-אפ
    this.showAttributesPopup = true;
  }

  closeAttributesPopup() {
    this.showAttributesPopup = false;
  }

  addPatientAttribute(){
    
const patientAttributeData = {
  attributeName: this.addAttributeForm.value.attributeName,
  score: this.addAttributeForm.value.score
}

  this.patientAttributeService.addPatientAttribute(patientAttributeData).subscribe({
    next: (response) => {
      console.log('Attribute added successfully:', response);
      this.snackBar.open(`מאפיין נוסף בהצלחה!`, 'סגור', { duration: 3000, panelClass: ['success-snackbar'] });
   
    },
    error: (err) => {
      console.error(err);
      this.snackBar.open('שגיאה בהוספת מאפיין', 'סגור', { duration: 3000, panelClass: ['error-snackbar'] });
    }
  });
}

getAttributes() {
  this.patientAttributeService.getAttributes().subscribe({
    next: (data: any) => {
      console.log('Attributes received:', data);
      this.attributes = data;
    },
    error: (err) => {
      console.error('Error fetching attributes:', err);
      this.snackBar.open('error in loading data' , 'close', { duration: 3000 });
    }
  });
}
confirmDelete(clientAttributeId: number) {
  const confirmDelete = confirm("are you sure you want to delete?");
  if (confirmDelete) {
    this.deletePatientAttribute(clientAttributeId);
  }
}

deletePatientAttribute(clientAttributeId: number) {
  this.patientAttributeService.deletePatientAttribute(clientAttributeId).subscribe({
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

  }



