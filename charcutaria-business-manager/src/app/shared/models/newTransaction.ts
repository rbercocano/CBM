export interface NewTransaction {
    amount: number;
    date: string;
    description: string;
    merchantName: string;
    transactionTypeId: number;
    orderId: number | null;
}