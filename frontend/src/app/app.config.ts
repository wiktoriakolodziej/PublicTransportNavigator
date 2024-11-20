import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideAnimations } from "@angular/platform-browser/animations";
import { authInterceptor } from './auth-interceptor';


export const apiUrl = 'https://localhost:7120';
export const webUrl = 'https://localhost:4200';
export const ETagName = "ETag";

export const appConfig: ApplicationConfig = {
	providers: [
		provideZoneChangeDetection({ eventCoalescing: true }),
		provideRouter(routes),
		provideHttpClient(withInterceptors([authInterceptor])), provideAnimationsAsync(), provideAnimationsAsync(),
		provideAnimations()
	]
};

