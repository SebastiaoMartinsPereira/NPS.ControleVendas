// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
import { env } from './.env';

export const environment = {
  production: false, 
  hmr: true,
  version: env.npm_package_version + '-dev',
  serverUrl: 'https://localhost:7167/api/',
  versionApi: '',
  defaultLanguage: 'pt-BR',
  supportedLanguages: [
    'pt-BR'
  ],
  ASPNETCORE_HTTPS_PORT : '7167',
  ASPNETCORE_URLS:'https://localhost'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
