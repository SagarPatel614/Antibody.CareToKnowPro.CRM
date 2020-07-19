export interface IProblemDetails { 
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    readonly extensions?: { [key: string]: any; };
}
