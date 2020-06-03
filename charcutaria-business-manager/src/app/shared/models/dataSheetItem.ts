import { RawMaterial } from './rawMaterial';

export interface DataSheetItem {
    dataSheetItemId: number;
    dataSheetId: number;
    rawMaterialId: number;
    rawMaterial: RawMaterial;
    percentage: number;
    additionalInfo: string;
    isBaseItem: boolean;
}