import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Contact } from '../models/contact';

@Component({
  selector: 'app-contact-info',
  templateUrl: './contact-info.component.html',
  styleUrls: ['./contact-info.component.scss']
})
export class ContactInfoComponent {
  @Input("contact") contact: Contact;
  @Output() edit: EventEmitter<Contact> = new EventEmitter();
  @Output() delete: EventEmitter<Contact> = new EventEmitter();
  editContact() {
    this.edit.emit(this.contact);
  }
  deleteContact() {
    this.delete.emit(this.contact);
  }
}
