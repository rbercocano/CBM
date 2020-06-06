export interface IConfirmModal {
    message: string;
    item: any;
    confirmAction(item: any);
}