import { Product } from './product';
import { MeasureUnit } from './measureUnit';
import { OrderItemStatus } from './orderItemStatus';

export interface OrderItemDetails {
    product: Product;
    measureUnit: MeasureUnit;
    orderItemStatus: OrderItemStatus;
    quantity: number;
    itemNumber: number;
    additionalInfo: string;
    discount: number;
    priceAfterDiscount: number;
    originalPrice: number;
    productPrice: number;
}