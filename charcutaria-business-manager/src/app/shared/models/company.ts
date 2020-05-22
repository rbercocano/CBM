import { Customer } from './customerBase';

export interface Company extends Customer {
    name: string;
    dbaName: string;
    cnpj: string;
}