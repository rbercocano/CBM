import { Product } from './product';
import { MeasureUnit } from './measureUnit';
import { OrderItemStatus } from './orderItemStatus';

export interface ProductQuote {
    product: Product;
    discount: number;
    price: number;
    finalPrice: number;
    quantity: number;
    measureUnit: MeasureUnit;
    additionalInfo: string;
    orderItemStatus: OrderItemStatus;
    orderItemId: number | null;
}