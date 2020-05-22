export interface OrderItem {
    orderItemId: number;
    orderId: number;
    productId: number;
    product: string;
    quantity: number;
    orderItemStatusId: number;
    measureUnitId: number;
    measureUnit: string;
    measureUnitShort: string;
    createdOn: string;
    lastUpdated: string;
    additionalInfo: string;
    originalPrice: number;
    discount: number;
    priceAfterDiscount: number;
}