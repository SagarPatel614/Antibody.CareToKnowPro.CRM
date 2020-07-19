import { Campaign } from "./Campaign";

export interface Message
    {
        id?: string;
        deduplicateId?: string;
        msgTemplateId?: string;
        actionId?: string;
        customerId?: string;
        recipient?: string;
        subject?: string;
        metrics?: Metrics;
        created?: Date | string | null;
        failureMessage?: string;
        newsletterId?: string;
        contentId?: string;
        campaignId?: string;
        broadcastId?: string;
        messageType?: string;
        forgotten?: boolean | null;
        campaign?: Campaign;
    }

    export interface Metrics
    {
       events?: MessageEvent[];
    }

    export interface MessageEvent
    {
        name?: string;
        eventDateTime?: Date | string | null;
    }
