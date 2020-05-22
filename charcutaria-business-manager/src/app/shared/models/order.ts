import { OrderItem } from './orderItem';

export interface Order {
    orderId: number;
    customerId: number;
    createdOn: string;
    lastUpdated: string;
    completeBy: string;
    orderStatusId: number;
    paymentStatusId: number;
    freightPrice: number;
    orderItems: OrderItem[];
    totalPrice: number;
}