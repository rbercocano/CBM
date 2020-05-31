import { DataSheetItem } from './dataSheetItem';

export interface DataSheet {
    dataSheetId: number;
    productId: number;
    procedureDescription: string;
    dataSheetItems: DataSheetItem[];
}