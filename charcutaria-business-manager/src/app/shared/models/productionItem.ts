import { DataSheetItem } from './dataSheetItem';

export interface ProductionItem extends DataSheetItem {
    quantity: number;
    cost: number;
}