import { IUnsubscribeReasons as UnsubscribeReasons } from './unsubscribeReasons';
import { IUser as User } from './user';

export interface IUserUnsubscribe { 
    unsubscribeId?: number;
    userId?: number;
    reasonId?: number;
    other?: string;
    reason?: UnsubscribeReasons;
    user?: User;
}
