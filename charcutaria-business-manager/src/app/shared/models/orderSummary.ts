export interface OrderSummary {
    orderId: number;
    orderNumber: number;
    name: string,
    socialIdentifier: string;
    customerTypeId: number;
    paymentStatusId: number;
    orderStatusId: number;
    paymentStatus: string;
    orderStatus: string;
    completeBy: string;
    createdOn: string;
    paidOn: string;
    finalPrice: number;
    customerId: number;
}