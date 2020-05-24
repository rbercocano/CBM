export interface NewOrderItem {
    productId: number;
    quantity: number;
    orderItemStatusId: number;
    additionalInfo: string;
    discount: number;
    measureUnitId: number;
    orderItemId: number;
    orderId: number;
}