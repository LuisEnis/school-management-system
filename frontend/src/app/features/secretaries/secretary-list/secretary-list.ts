import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Observable, startWith, Subject, switchMap } from 'rxjs';

import { UserService } from '../../../core/services/user.service';
import { UserDto } from '../../../core/models/users/user.dto';
import { AuthService } from '../../../core/services/auth.service';


@Component({
  selector: 'app-secretary-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './secretary-list.html',
  styleUrl: './secretary-list.css'
})
export class SecretaryList implements OnInit {

  private reload$ = new Subject<void>();

  secretaries$!: Observable<UserDto[]>;


  constructor(
    private userService: UserService,
    public authService: AuthService
  ) {}


  ngOnInit(): void {

    this.secretaries$ =
      this.reload$
        .pipe(
          startWith(null),
          switchMap(() =>
            this.userService.getSecretaries()
          )
        );

  }


  delete(id: number): void {

    if (!confirm(
      'Are you sure you want to delete this secretary?'
    ))
      return;


    this.userService
      .delete(id)
      .subscribe({

        next: () => {

          this.reload$.next();

        },

        error: error => {

          console.error(error);

        }

      });

  }

}