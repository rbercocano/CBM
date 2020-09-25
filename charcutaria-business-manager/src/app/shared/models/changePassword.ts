export interface ChangePassword {
    userId: number;
    password: string;
    newPassword: string;
    newPasswordConfirmation: string;
}