export interface JWTUserInfo {
    userId: number;
    corpClientId: number | null;
    username: string;
    name: string;
    active: boolean;
    roleId: Number;
    createdOn: Date;
    lastUpdated: Date | null;
    dBAName: string;
    companyName: string;
}
