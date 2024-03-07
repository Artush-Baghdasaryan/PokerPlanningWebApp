import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HTTP_INTERCEPTORS
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { GuestService } from '../guest.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private guestService: GuestService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    console.log("intercepting!!!");
    let headers = req.headers.set('Content-Type', 'application/json');
    headers = headers.set('Accept', 'text/plain');

    const request = req.clone({ headers: headers });
    return next.handle(request);
  }
}

export const authInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
];
