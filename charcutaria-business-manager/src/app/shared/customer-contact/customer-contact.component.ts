import { Component, OnInit, Input } from '@angular/core';
import { Contact } from '../models/contact';
import { ContactType } from '../models/contactType';
import { NgbModalRef, ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Customer } from '../models/customerBase';
import { NotificationService } from '../services/notification/notification.service';
import { DomainService } from '../services/domain/domain.service';
import { CustomerService } from '../services/customer/customer.service';
import { of, forkJoin, Observable } from 'rxjs';
import { flatMap } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-customer-contact',
  templateUrl: './customer-contact.component.html',
  styleUrls: ['./customer-contact.component.scss']
})
export class CustomerContactComponent implements OnInit {
  @Input("customer") customer: Customer;
  public contactTypes: ContactType[] = [];
  public contacts: Contact[] = [];
  public selectedContactType: ContactType;
  public contact: Contact = { contactTypeId: 1, contact: '' } as Contact;
  private modal: NgbModalRef;
  private modalConfirm: NgbModalRef;
  constructor(private notificationService: NotificationService,
    private domainService: DomainService,
    private customerService: CustomerService,
    private modalService: NgbModal,
    private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
  }
  init(): Observable<boolean> {
    let oContactTypes = this.domainService.GetContactTypes();
    let oContacts = this.customer.customerId ? this.customerService.GetContacts(this.customer.customerId) : of(new Array<Contact>());
    let result = forkJoin(oContactTypes, oContacts).pipe(flatMap(r => {
      this.contactTypes = r[0];
      this.selectedContactType = this.contactTypes[0];
      this.contacts = r[1];
      return of(true);
    }));
    return result;
  }
  saveContact() {
    if (!this.contact.contact) return;
    this.modal.close();
    this.spinner.show();
    this.contact.customerId = this.customer.customerId;
    this.contact.contactTypeId = this.selectedContactType.contactTypeId;
    this.contact.contactType = this.selectedContactType.description;
    this.contact.contactIcon = this.selectedContactType.icon;
    if (!this.contact.customerContactId) {
      this.customerService.AddContact(this.contact).subscribe(r => {
        this.contact.customerContactId = r;
        this.contacts.push({ ...this.contact });
        this.notificationService.showSuccess('Sucesso', 'Contato salvo com sucesso.');
      }, (e) => {
        this.notificationService.notifyHttpError(e);
      }, () => {
        this.spinner.hide();
      });
    }
    else {
      this.customerService.UpdateContact(this.contact).subscribe(r => {
        this.notificationService.showSuccess('Sucesso', 'Contato salvo com sucesso.');
      }, (e) => {
        this.notificationService.notifyHttpError(e);
      }, () => {
        this.spinner.hide();
      });
    }
  }
  open(content, c: Contact | null) {
    this.contact = c ?? { contactTypeId: 1, contact: '' } as Contact;
    this.selectedContactType = c == null ? this.contactTypes[0] : this.contactTypes.filter(ct => ct.contactTypeId == c.contactTypeId)[0];
    this.modal = this.modalService.open(content, { size: 'lg' });
  }
  onEdit(contact: Contact, content) {
    this.open(content, contact);
  }
  onDelete(contact: Contact, delConfirm) {
    this.contact = contact;
    this.modalConfirm = this.modalService.open(delConfirm, {});
  }
  confirmDelete() {
    this.spinner.show();
    this.customerService.DeleteContact(this.contact.customerContactId).subscribe(r => {
      this.contacts = this.contacts.filter(t => t !== this.contact);
      this.notificationService.showSuccess('Sucesso', 'Contato excluído com sucesso.');
    }, (e) => {
      this.notificationService.notifyHttpError(e);
    }, () => {
      this.modalConfirm.close();
      this.spinner.hide();
    });
  }
}
