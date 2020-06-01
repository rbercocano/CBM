export interface DataSheetItem {
    dataSheetItemId: number;
    dataSheetId: number;
    rawMaterialId: number;
    rawMaterial: string;
    percentage: number;
    additionalInfo: string;
    isBaseItem: boolean;
}