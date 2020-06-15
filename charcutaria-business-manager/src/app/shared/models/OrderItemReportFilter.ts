
import { OrderStatus } from './orderStatus';
import { OrderItemStatus } from './orderItemStatus';

export class OrderItemReportFilter {
    orderNumber: number;
    public itemStatus: OrderItemStatus[];
    public orderStatus: OrderStatus[];
    completeByFrom: string;
    completeByTo: string;
    customer: string;
    orderBy: number;
    direction: number;
}