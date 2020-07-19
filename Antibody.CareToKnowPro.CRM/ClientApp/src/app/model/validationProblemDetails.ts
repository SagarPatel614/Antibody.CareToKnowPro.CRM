export interface IValidationProblemDetails { 
    readonly errors?: { [key: string]: Array<string>; };
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    readonly extensions?: { [key: string]: any; };
}
