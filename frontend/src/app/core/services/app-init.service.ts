import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {

  constructor(
    private authService: AuthService
  ) {}


  initialize(): Promise<void> {

    const token =
      this.authService.getToken();


    if (!token) {
      return Promise.resolve();
    }


    return firstValueFrom(
      this.authService.loadCurrentUser()
    )
    .then(() => {})
    .catch(() => {

      this.authService.logout();

    });

  }
}