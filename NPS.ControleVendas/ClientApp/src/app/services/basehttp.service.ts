import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';  
import { map, catchError, timeout } from 'rxjs/operators';
import { Observable, throwError, EMPTY } from 'rxjs'; 
import { Logger } from '../core/logger.service';  
import * as moment from 'moment';
import { Lista } from '../models/lista.model';
import { environment } from 'src/environments/environment';

const log = new Logger('BaseService');

export abstract class BaseHttpService<T>  {

  static timer: any;
  public redefinirTimeOut: number;

  constructor(
    public http: HttpClient,
    public router: Router
  ) { }

  protected getAll(url: string): Observable<T[]> {
    return this.get(url);
  }

  protected getById(url: string, _id: number): Observable<T> {
    return this.get(`${url}/${_id}`);
  }


  protected getFiltered(url: string, filter: any): Observable<Lista<T>> {
    const f: any = JSON.parse(JSON.stringify(filter));
    delete f.total;
    if (f.pageSize === 0) {
      delete f.pageSize;
    }
    const sendBody: string = JSON.stringify(f);
    return this.post(url, sendBody);
  }
 
  protected put(url: string, body: any, timeout: number = undefined): Observable<any> {

    if (timeout) this.redefinirTimeOut = timeout;
    return this.send('put', url, body);
  }

  protected post(url: string, body: any, timeout: number = undefined): Observable<any> {
    if (timeout) this.redefinirTimeOut = timeout;
    return this.send('post', url, body);
  }

  protected patch(url: string, body: any): Observable<any> {
    return this.send('patch', url, body);
  }

  protected postFullUrl(url: string, body: any): Observable<any> {
    let time: any;
    if (!environment.production) {
      log.info('postFullUrl-Post Full url: ', url, ' | body: ', body);
      time = moment();
    }

    return this.http
      .post(url, JSON.stringify(body), {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      }).pipe(
        map((a: HttpResponse<any>) => this.extractData(a, url, time)),
        catchError((r: any) => this.handleError(r, url))
      );
  }

  protected get(url: string): Observable<any> {
    url = `${environment.serverUrl}${environment.versionApi}/${url}`;
    let time: any;
    if (!environment.production) {
      log.info(`get-Get url: ${url}`);
      time = moment();
    }
    const that = this;
    return this.http
      .get(url, { observe: 'response' }).pipe(
        map((a: HttpResponse<any>) => this.extractData(a, url, time)),
        catchError((r: any) => this.handleError(r, url))
      );
  }

  protected getHeadersUpload(): any {
    const headers: any = { Accept: 'application/json' };
    const authHeader: string = sessionStorage.getItem('soue-token');
    if (authHeader) {
      Object.assign(headers, { 'soue-token': authHeader });
    }
    return headers;
  }

  protected extractData(res: any, path: string, time?: Date): any {
    if (res.status === 403) {
      localStorage.setItem('error', res.error);
      this.router.navigate(['/login', {
        returnUrl: this.router.routerState.snapshot.url.indexOf('returnUrl') > -1
          ? this.router.routerState.snapshot.url.split('returnUrl=')[1]
          : this.router.routerState.snapshot.url
      }]);
      return throwError('Go to login');
    }
    if (res.status === 405) {
      log.error('response', res);
      return {};
    }

    if (!environment.production) {
      // const bd: string = JSON.stringify(res);
      log.info('response ', path, ' duration: ', moment().diff(time, 'millisecond'), ' | body: ', res);
    }
    const bd = res.body;

    // const keys = res.headers.keys();
    // const headers = keys.map(key => `${key}: ${res.headers.get(key)}`);
    clearTimeout(BaseHttpService.timer);
    BaseHttpService.timer = setTimeout(() => {
      localStorage.setItem('error', 'api.warnings.token_expired');
      this.router.navigate(['/login', {
        returnUrl: this.router.routerState.snapshot.url.indexOf('returnUrl') > -1
          ? this.router.routerState.snapshot.url.split('returnUrl=')[1]
          : this.router.routerState.snapshot.url
      }]);
    }, 20 * 60 * 1000);

    if (res.headers) {
      const tk = res.headers.get('soue-token');
      if (tk) {
        log.debug(' TROCOU TOKEN ');
        sessionStorage.setItem('soue-token', tk);
      }
      const dbDate = res.headers.get('soue-db-date');
      log.debug('dbDate', dbDate);
      if (dbDate) {
        localStorage.setItem('dbDate', '' + moment().diff(moment(dbDate), 'milliseconds'));
      }

      const pwaV = res.headers.get('pwa-version');
      if (pwaV) {
        const v = localStorage.getItem('pwa-version');
        localStorage.setItem('pwa-version', pwaV);
        if (v && v !== pwaV) {
          localStorage.setItem('success', 'api.success.system_updated');
          document.location.reload();
        }
      }
    }

    if (res.error) {
      return res;
    }

    if (res.headers && res.headers.get('ttl')) {
      return new Lista<T>({
        total: res.headers.get('ttl'),
        lst: bd
      });
    }

    if (bd === false) {
      return false;
    }
    if (bd === null) {
      return {};
    }

    return ((!bd.data && bd.data !== false) || typeof bd.data === 'string')
      ? (function () {
        if (typeof (bd) === "number") { return bd }
        return bd || {}
      })()
      : bd.data === false ? false : bd.data || {};
  }

  protected handleError(error: any, path: string): Observable<any> {
    error = error || {};
    const body: any = error;
    const errMsg: string = body
      ? (body.error && (typeof body.error === 'string' || typeof body.error === 'object') ? body.error : body.message)
      : (error.status ? `${error.status} - ${error.statusText}` : 'Server error');
    if (error.status === 403) {
      localStorage.setItem('error', errMsg);
      this.router.navigate(['/login', {
        returnUrl: this.router.routerState.snapshot.url.indexOf('returnUrl') > -1
          ? this.router.routerState.snapshot.url.split('returnUrl=')[1]
          : this.router.routerState.snapshot.url
      }]);
      return throwError('Go to login');
    }
    log.error(path, ' | error', error, ' | response body: ', body);
    return throwError(errMsg);
  }

  private send(verb: string, url: string, body: any): Observable<any> {
    let time: any;
    url = `${environment.serverUrl}${environment.versionApi}/${url}`;
    if (!environment.production) {
      log.info('send-', verb, ' url: ', url, '  | body: ', body
        && typeof body === 'string'
        && body.trim().indexOf('{') === 0
        ? JSON.parse(body)
        : body);
      time = moment();
    }

    let ret = this.http[verb](url, body, {
      observe: 'response',
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    }).pipe(
      map((a: HttpResponse<any>) => this.extractData(a, url, time)),
      catchError((r: any) => this.handleError(r, url)),
    );

    if (this.redefinirTimeOut) {
      ret.pipe(timeout(this.redefinirTimeOut))
    }

    return ret;

  }
}
