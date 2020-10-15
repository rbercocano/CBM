import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from 'src/app/shared/models/user';
import { AuthService } from 'src/app/shared/services/auth/auth.service';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { UserService } from 'src/app/shared/services/user/user.service';
import * as moment from 'moment';
import { DomainService } from 'src/app/shared/services/domain/domain.service';
import { forkJoin } from 'rxjs';
import { MeasureUnit } from 'src/app/shared/models/measureUnit';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  @ViewChild('f', { static: true }) form: NgForm;
  private currentData: User = {} as User;
  public user: User = {} as User;
  public maxDate = new Date();
  public editing = false;
  public mass: MeasureUnit[] = [];
  public volumes: MeasureUnit[] = [];
  constructor(
    private spinner: NgxSpinnerService,
    private notificationService: NotificationService,
    private userService: UserService,
    private authService: AuthService,
    private domainService: DomainService) { }

  ngOnInit(): void {
    this.spinner.show();
    let oM = this.domainService.GetMeasureUnits();
    let oU = this.userService.GetUser(this.authService.userData.userId);
    forkJoin([oM, oU]).subscribe(r => {
      r[0].forEach((v, i) => {
        if (v.measureUnitTypeId == 1)
          this.mass.push(v);
        if (v.measureUnitTypeId == 2)
          this.volumes.push(v);
      });
      this.user = r[1];
      this.currentData = { ...this.user };
      this.spinner.hide();
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });
  }
  back() {

  }
  public cancel(): void {
    this.editing = false;
    this.user = { ...this.currentData };
  }
  save() {
    if (this.form.invalid) return;
    this.spinner.show();
    this.userService.Update(this.user).subscribe(r => {
      this.currentData = { ...this.user };
      this.editing = false;
      this.spinner.hide();
      this.notificationService.showSuccess('Sucesso', 'Dados atualizados com sucesso');
    }, e => {
      this.spinner.hide();
      this.notificationService.notifyHttpError(e);
    });

  }
  public get dob(): string {
    if (this.user.dateOfBirth) {
      let date = moment(this.user.dateOfBirth);
      let dob = {
        day: parseInt(date.format("DD")),
        month: parseInt(date.format("MM")),
        year: parseInt(date.format("YYYY"))
      };
      return `${dob.day}/${dob.month}/${dob.year}`;
    }
    return '';
  }
}
