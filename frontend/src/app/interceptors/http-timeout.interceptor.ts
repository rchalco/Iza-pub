import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, timer } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { NetworkQualityService } from '../services/network-quality.service';

const DEFAULT_TIMEOUT_MS = 5000;

export const httpTimeoutInterceptor: HttpInterceptorFn = (
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> => {
  const networkService = inject(NetworkQualityService);
  const router = inject(Router);

  const timeoutMs = networkService.getAdaptiveTimeout(DEFAULT_TIMEOUT_MS);

  return next(req).pipe(
    timeout(timeoutMs),
    catchError((error: HttpErrorResponse) => {
      if (error.status === 408 || (error as any).name === 'TimeoutError') {
        console.warn(`Timeout (${timeoutMs}ms) en: ${req.method} ${req.url}`);
        console.warn(`Red: ${networkService.getNetworkTypeLabel()}`);

        const isLogin = req.url.toLowerCase().includes('login');
        if (isLogin) {
          router.navigate(['/login']);
        }
      }
      return throwError(() => error);
    })
  );
};