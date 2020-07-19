import { IUserUnsubscribe as UserUnsubscribe } from './userUnsubscribe';

export interface IUnsubscribeReasons { 
    reasonId?: number;
    reason?: string;
    reasonFr?: string;
    userUnsubscribe?: Array<UserUnsubscribe>;
}
