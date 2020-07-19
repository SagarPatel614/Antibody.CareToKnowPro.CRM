import { IUser as User } from './user';

export interface IProvince { 
    provinceId?: number;
    frenchName: string;
    englishName: string;
    abbreviation?: string;
    user?: Array<User>;
}
