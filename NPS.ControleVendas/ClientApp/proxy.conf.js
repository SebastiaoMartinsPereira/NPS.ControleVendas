const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7167';

const PROXY_CONFIG = [
  {
    context: [
      "/api/*"
   ],
    proxyTimeout: 10000,
    target: "https://localhost:7167",
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  },
  {
    "/api/*": {
    "target": "https://localhost:7167",
    "secure": false,
    headers: {
        Connection: 'Keep-Alive'
      }
    }
  }
]

module.exports = PROXY_CONFIG;
