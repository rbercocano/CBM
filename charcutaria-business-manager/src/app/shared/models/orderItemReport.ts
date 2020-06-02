export interface OrderItemReport {
    orderId: number;
    orderNumber: number;
    customerTypeId: number;
    orderStatus: string;
    customer: string;
    socialIdentifier: string;
    itemNumber: number;
    product: string;
    quantity: number;
    measureUnit: string;
    orderItemStatus: string;
    finalPrice: number;
    lastStatusDate: string;
    completeBy: string;
}