import { Product } from './product';
import { MeasureUnit } from './measureUnit';

export interface ProductQuote {
    product: Product;
    discount: number;
    price: number;
    finalPrice: number;
    quantity: number;
    measureUnit: MeasureUnit;
    additionalInfo: string;
}