import { Component, OnInit } from '@angular/core';
import { QueueService } from '../services/queue.service';
import { Subscription, interval } from 'rxjs';
import { MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon'; // ייבוא המודול
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-all-queue',
  standalone: true,
  imports: [MatIconModule,CommonModule],
  templateUrl: './all-queue.component.html',
  styleUrl: './all-queue.component.css'
})
export class AllQueueComponent {
  patients: any[] = [];
  private refreshSubscription!: Subscription;
  avgWaiting:number|null=null;


  constructor(private queueService: QueueService,private dialogRef: MatDialogRef<AllQueueComponent>) {}

  ngOnInit(): void {
    this.loadQueue();
      // ריענון הנתונים כל 5 שניות
      this.refreshSubscription = interval(5000).subscribe(() => {
        this.loadQueue();
      });

      this.calculateEstimatedWaitTime();
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
    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    if (this.refreshSubscription) {
      this.refreshSubscription.unsubscribe(); // מניעת זליגת זיכרון
    }
  }
  calculateEstimatedWaitTime() {
  
    this.queueService.getQueueAvarage().subscribe({
 next:(avarage)=>{
  this.avgWaiting=avarage;
  console.log(`מספר המטופלים בתור: ${avarage}`)
 },
 error: (err) => {
   console.error("שגיאה בקבלת גודל התור", err);
 }
    }) 
 }

  

}
