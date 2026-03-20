import { enableProdMode, provideZoneChangeDetection } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import config from 'devextreme/core/config';
import { licenseKey } from './devextreme-license';

if (environment.production) {
  enableProdMode();
}

if (licenseKey?.trim()) {
  config({ licenseKey });
}
platformBrowserDynamic().bootstrapModule(AppModule, { applicationProviders: [provideZoneChangeDetection()], })
  .catch(err => console.log(err));
