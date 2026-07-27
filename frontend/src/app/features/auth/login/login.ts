import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  email = '';
  password = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin(): void {

      this.authService
          .login({
              email: this.email,
              password: this.password
          })
          .subscribe({
              next: () => {

                  this.authService
                      .loadCurrentUser()
                      .subscribe({
                          next: () => {

                              this.router.navigate(['/']);

                          },
                          error: () => {

                              this.errorMessage =
                                  'Could not load user information.';

                          }
                      });

              },
              error: () => {

                  this.errorMessage =
                      'Invalid email or password';

              }
          });

  }
}