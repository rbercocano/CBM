import { ProductionItem } from './productionItem';

export interface ProductionSummary {
    productionItems: ProductionItem[];
    productionCost: number;
    salePrice: number;
    profit: number;
    profitPercentage:number;
}