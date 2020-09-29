
import { OrderStatus } from './orderStatus';
import { OrderItemStatus } from './orderItemStatus';

export class OrderItemReportFilter {
    orderNumber: number;
    itemStatus: OrderItemStatus[];
    orderStatus: OrderStatus[];
    products: number[];
    completeByFrom: string;
    completeByTo: string;
    customer: string;
    orderBy: number;
    direction: number;
    massUnitId: number;
    volumeUnitId: number;
    customerId:number;
    measureUnitId: number;
}