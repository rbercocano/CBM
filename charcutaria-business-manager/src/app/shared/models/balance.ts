export interface Balance {
    transactionDate: string;
    amount: number;
    remainingBalance: number;
    description: string;
    merchantName: string;
    transactionTypeId: number;
    orderId: number;
    transactionType: string;
    showDetails: boolean;
    userId: number;
    username: string;
    userFullName: string;
}