import { DataSheetItem } from './dataSheetItem';
import { MeasureUnit } from './measureUnit';

export interface ProductionItem extends DataSheetItem {
    quantity: number;
    cost: number;
    measureUnit: MeasureUnit;
}