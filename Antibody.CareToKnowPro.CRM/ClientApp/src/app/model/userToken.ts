import { IUser as User } from './user';

export interface IUserToken { 
    tokenId?: number;
    userId?: number;
    email: string;
    token: string;
    createdOn?: Date;
    type?: number;
    user?: User;
}
