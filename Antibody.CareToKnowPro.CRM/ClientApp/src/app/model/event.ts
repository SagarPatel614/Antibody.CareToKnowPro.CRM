import { IEventEntity as EventEntity } from './eventEntity';
import { ILoginProfile as LoginProfile } from './loginProfile';

export interface IEvent { 
    eventId?: number;
    eventType?: number;
    eventDateUtc?: Date;
    loginProfileId?: number;
    eventNotes?: string;
    loginProfile?: LoginProfile;
    eventEntity?: Array<EventEntity>;
}
