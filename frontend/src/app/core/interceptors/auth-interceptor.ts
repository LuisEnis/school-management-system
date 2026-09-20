import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();

  const request = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;


  return next(request)
    .pipe(
      catchError((error: HttpErrorResponse) => {

        if (
          error.status === 401 &&
          !req.url.includes('/auth/login') &&
          !req.url.includes('/auth/refresh') &&
          !req.url.includes('/auth/logout')
        ) {

          const currentToken =
            authService.getToken();

          const requestToken =
            request.headers
              .get('Authorization')
              ?.replace('Bearer ', '');


          if (
            currentToken &&
            requestToken &&
            currentToken !== requestToken
          ) {

            const retryRequest =
              req.clone({
                setHeaders: {
                  Authorization:
                    `Bearer ${currentToken}`
                }
              });


            return next(retryRequest);
          }

          return authService
            .refreshToken()
            .pipe(
              switchMap(response => {

                const retryRequest =
                  req.clone({
                    setHeaders: {
                      Authorization:
                        `Bearer ${response.token}`
                    }
                  });

                return next(retryRequest);

              }),

              catchError(refreshError => {

                authService.clearSession();

                router.navigate(['/login']);

                return throwError(() => refreshError);

              })
            );

        }


        return throwError(() => error);

      })
    );
};