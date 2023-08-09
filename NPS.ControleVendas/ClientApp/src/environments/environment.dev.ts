import { env } from './.env';
export const environment = {
  production: false, 
  hmr: true,
  version: env.npm_package_version + '-dev',
  serverUrl: 'https://localhost:7167/api',
  versionApi: '',
  defaultLanguage: 'pt-BR',
  supportedLanguages: [
    'pt-BR'
  ],
  ASPNETCORE_HTTPS_PORT : '7167',
  ASPNETCORE_URLS:'https://localhost'
};