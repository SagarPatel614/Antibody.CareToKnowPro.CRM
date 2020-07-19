import { IEventEntity as EventEntity } from './eventEntity';

export interface IEventEntityProperty { 
    eventEntityPropertyId?: number;
    eventEntityId?: number;
    propertyName: string;
    originalValue?: string;
    newValue?: string;
    eventEntity?: EventEntity;
}
