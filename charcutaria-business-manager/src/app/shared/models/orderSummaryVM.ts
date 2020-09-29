import { OrderItemDetails } from './orderItemDetails';
import { OrderItemReport } from './orderItemReport';
import { OrderSummary } from './orderSummary';

export class OrderSummaryVM implements OrderSummary {
    orderId: number;
    orderNumber: number;
    name: string;
    socialIdentifier: string;
    customerTypeId: number;
    paymentStatusId: number;
    orderStatusId: number;
    paymentStatus: string;
    orderStatus: string;
    completeBy: string;
    CreatedOn: string;
    paidOn: string;
    finalPrice: number;
    isExpanded: boolean = false;
    orderItems: OrderItemReport[] = [];
    customerId: number;
}