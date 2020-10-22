import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CorpClientRoutingModule } from './corp-client-routing.module';
import { EditCorpClientComponent } from './edit-corp-client/edit-corp-client.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserRoutingModule } from '../user/user-routing.module';


@NgModule({
  declarations: [EditCorpClientComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    SharedModule,
    CorpClientRoutingModule
  ]
})
export class CorpClientModule { }
