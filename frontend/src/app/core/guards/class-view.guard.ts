import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

import { AuthService } from '../services/auth.service';

export const classViewGuard: CanActivateFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  if (
    authService.isDirector() ||
    authService.isSecretary() ||
    authService.isTeacher()
  ) {
    return true;
  }

  router.navigate(['/dashboard']);
  return false;
};