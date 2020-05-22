import { Customer } from './customerBase';

export interface Person extends Customer{
    name: string;
    lastName: string;
    dateOfBirth: string;
    cpf: string;
}