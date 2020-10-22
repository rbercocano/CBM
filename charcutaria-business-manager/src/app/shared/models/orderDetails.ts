import { OrderStatus } from './orderStatus';
import { PaymentStatus } from './paymentStatus';
import { MergedCustomer } from './mergedCustomer';
import { OrderItemDetails } from './orderItemDetails';
import { Transaction } from './transaction';

export interface OrderDetails {
    orderId: number;
    orderNumber: number;
    createdOn: string;
    lastUpdated: string;
    completeBy: string;
    paidOn: string;
    orderStatusId: number;
    orderStatus: OrderStatus;
    paymentStatusId: number;
    paymentStatus: PaymentStatus;
    freightPrice: number;
    customer: MergedCustomer;
    orderItems: OrderItemDetails[];
    discountTotal: number;
    itemsTotal: number;
    itemsTotalAfterDiscounts: number;
    orderTotal: number;
    itemsTotalCost: number;
    itemsTotalProfit: number;
    payments: number;
    tips: number;
    remainingBalance: number;
    transactions: Transaction[];
}