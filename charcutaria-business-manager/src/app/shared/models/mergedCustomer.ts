import { Contact } from './contact';

export interface MergedCustomer {
    name: string;
    socialIdentifier: string;
    customerType: string;
    customerId: number;
    customerTypeId: number;
    contacts: Contact[];
}