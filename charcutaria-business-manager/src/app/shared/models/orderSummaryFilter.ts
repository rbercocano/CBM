import { PaymentStatus } from './paymentStatus';
import { OrderStatus } from './orderStatus';

export class orderSummaryFilter {
    public customer: string;
    public createdOnFrom: string;
    public createdOnTo: string;
    public paidOnFrom: string;
    public paidOnTo: string;
    public completeByFrom: string;
    public completeByTo: string;
    public paymentStatus: PaymentStatus[];
    public orderStatus: OrderStatus[];
    public orderBy: number;
    public direction: number;
}