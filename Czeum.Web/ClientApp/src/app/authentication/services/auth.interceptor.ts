import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { Injectable } from '@angular/core';
import { switchMap, take } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.getAuthState().pipe(
      take(1),
      switchMap(res => {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ${res.accessToken}`
          }
        });

        return next.handle(req);
      })
    );
  }
}
