import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from "@angular/router";
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet,
    CommonModule,
    RouterLink,
    RouterLinkActive],
  templateUrl: './main-layout.html',
  styleUrl: './main-layout.css',
})
export class MainLayout {


  constructor(
    public authService: AuthService,
    private router: Router,
  ) {}

  get user() {

    return this.authService.getCurrentUser();

}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
