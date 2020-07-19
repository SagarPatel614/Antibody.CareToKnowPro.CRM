
import { IEvent as Event } from './event';
import { IEventEntityProperty as EventEntityProperty } from './eventEntityProperty';
import { IUser as User } from './user';

export interface IEventEntity { 
    eventEntityId?: number;
    eventId?: number;
    actionType?: number;
    position?: number;
    userId?: number;
    event?: Event;
    user?: User;
    eventEntityProperty?: Array<EventEntityProperty>;
}
