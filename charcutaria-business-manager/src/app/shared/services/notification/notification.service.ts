import { Injectable, TemplateRef } from '@angular/core';
import { Error } from '../../models/error'

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private defaultTimeout = 3000;
  constructor() { }

  toasts: any[] = [];

  private show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }
  remove(toast) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }

  showSuccess(header: string, msg: string, delay: number = null) {
    this.show(msg, {
      classname: 'bg-success text-light',
      delay: delay ?? this.defaultTimeout,
      autohide: true,
      headertext: header
    });
  }
  showError(header: string, msg: string, delay: number = null) {
    this.show(msg, {
      classname: 'bg-danger text-light',
      delay: delay ?? this.defaultTimeout,
      autohide: true,
      headertext: header
    });
  }

  showCustomToast(customTpl, delay: number = null) {
    this.show(customTpl, {
      classname: 'bg-info text-light',
      delay: delay ?? this.defaultTimeout,
      autohide: true
    });
  }

  notifyHttpError(err: any) {
    if (err.status == 400) {
      if (err.error instanceof Array) {
        let errors = <Array<string>>err.error;
        for (let e of errors) {
          let msg = `${e}`;
          console.log(msg);
          this.showError("Atenção", e, 2000);
        }
      }
    }
    else
      this.showError("Erro", err.message);
  }
}
