import { JWTUserInfo } from './jwtUserInfo';

export interface TokenInfo {
    userData: JWTUserInfo;
    authenticated: boolean;
    created: Date | null;
    expiration: Date | null;
    accessToken: string;
    message: string;
    refreshToken: string;
}