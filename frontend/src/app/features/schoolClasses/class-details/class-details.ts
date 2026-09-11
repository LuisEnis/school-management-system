import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

import { SchoolClassService } 
from '../../../core/services/schoolClass.service';

import { ClassDetailsDto }
from '../../../core/models/schoolClasses/class-details.dto';
import { Observable, startWith, Subject, Subscription, switchMap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';
import { SignalRService } from '../../../core/services/signalr.service';



@Component({
  selector:'app-class-details',
  standalone:true,
  imports:[
    CommonModule
  ],
  templateUrl:'./class-details.html',
  styleUrl:'./class-details.css'
})
export class ClassDetails implements OnInit, OnDestroy {

  private reload$ = new Subject<void>();
  private subscriptions = new Subscription();
  private classId!: number;


details$!: Observable<ClassDetailsDto>;


constructor(
 private route:ActivatedRoute,
 private schoolClassService:SchoolClassService,
 public authService: AuthService,
 private signalRService: SignalRService
){}




ngOnInit():void{

this.classId =
  Number(
    this.route.snapshot.paramMap.get('id')
  );

this.signalRService.startConnection();

this.details$ =
  this.reload$.pipe(
    startWith(null),
    switchMap(() =>
      this.schoolClassService.getDetails(this.classId)
    )
  );

  this.subscriptions.add(
    this.signalRService.studentClassAssigned$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.studentClassRemoved$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.teachingAssignmentCreated$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.teachingAssignmentRemoved$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.classUpdated$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.studentUpdated$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.teacherUpdated$
      .subscribe(() => this.reload$.next())
  );

  this.subscriptions.add(
    this.signalRService.subjectUpdated$
      .subscribe(() => this.reload$.next())
  );

}

ngOnDestroy(): void {
  this.subscriptions.unsubscribe();
}


}