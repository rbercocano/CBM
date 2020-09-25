import { OrderItemStatus } from './orderItemStatus';
import { Product } from './product';

export class SummarizedProductionFilter {
    itemStatus: OrderItemStatus[];
    products: Product[];
    volumeUnitId: number;
    massUnitId: number;
    orderBy: number;
    direction: number;
}