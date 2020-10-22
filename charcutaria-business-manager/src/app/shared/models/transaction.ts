export interface Transaction {
    transactionId: number;
    amount: number;
    date: string;
    description: string;
    merchantName: string;
    transactionTypeId: number;
    orderId: number;
    transactionType: string;
    transactionStatusId: number;
    isIncome: boolean;
}