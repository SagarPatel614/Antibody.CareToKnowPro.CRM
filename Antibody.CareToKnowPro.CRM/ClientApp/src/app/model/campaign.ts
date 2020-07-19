export interface Campaign
{
    id?: number;
    deduplicateId?: string;
    name?: string;
    campaignType?: string;
    created?: Date;
    updated?: Date;
    active?: boolean;
    state?: string;
    firstStarted?: Date;
    createdBy?: string;
    eventName?: string;
}
