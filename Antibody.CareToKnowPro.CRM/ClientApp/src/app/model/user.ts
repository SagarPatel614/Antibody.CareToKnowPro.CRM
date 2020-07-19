import { IEventEntity as EventEntity } from './eventEntity';
import { IProvince as Province } from './province';
import { IUserSpecialty as UserSpecialty } from './userSpecialty';
import { IUserToken as UserToken } from './userToken';
import { IUserUnsubscribe as UserUnsubscribe } from './userUnsubscribe';
import { Message } from './message';

export interface IUser {
  userId?: number;
  email?: string;
  firstName?: string;
  lastName?: string;
  graduationYear?: number;
  passwordHash?: string;
  encrypted?: string;
  userType?: number;
  provinceId?: number;
  preferredLanguage?: string;
  verified?: boolean;
  userGuid?: string;
  other?: string;
  registered?: boolean;
  province?: Province;
  eventEntity?: Array<EventEntity>;
  userSpecialty?: Array<UserSpecialty>;
  userToken?: Array<UserToken>;
  userUnsubscribe?: Array<UserUnsubscribe>;
  provinceList?: ProvinceList[];
  graduationList?: GraduationList[];
  specialities?: any;
  emailStatus?: string;
  secondaryEmails?: string;
  status?: string;
  notes?: string;
  createdBy?: string;
  dateCreated?: Date;
  dateModified?: Date;
  modifiedBy?: string;
  street1?: string;
  city?: string;
  postal?: string;
  country?: string;
  phoneNumber?: string;
  fax?:string;
  additionalInfo?:string;
  registrationModel?: string;
  messages? : Array<Message>
}

export interface IUserPagedResponse {
  results?: Array<IUser>;
  totalCount: number;
  recordsPerPage: number;
  currentPage: number;
}

export interface IRegister {
  firstName: string;
  lastName: string;
  email: string;
  secondaryEmails?: string;
  provinceId: number;
  graduationYear: string;
  preferredLanguage: string;
  specialtyIds: number[];
  other: string;
  provinceList: ProvinceList[];
  graduationList: GraduationList[];
  specialtyList: SpecialtyList[];
  specialities: any;
  errorMessage: string;
  isError: boolean;
  emailStatus?: string;
  status?: string;
  notes?: string;
  registrationModel?: string;
  createdBy?: string;
  dateCreated?: Date;
  dateModified?: Date;
  modifiedBy?: string;
  street1?: string;
  city?: string;
  postal?: string;
  country?: string;
  fax?:string;
  additionalInfo?:string;
  phoneNumber?: string;
}

export interface Group {
  disabled: boolean;
  name: string;
}

export interface ProvinceList {
  disabled: boolean;
  group: Group;
  selected: boolean;
  text: string;
  value: string;
}

export interface Group2 {
  disabled: boolean;
  name: string;
}

export interface GraduationList {
  disabled: boolean;
  group: Group2;
  selected: boolean;
  text: string;
  value: string;
}

export interface Group3 {
  disabled: boolean;
  name: string;
}

export interface SpecialtyList {
  disabled: boolean;
  group: Group3;
  selected: boolean;
  text: string;
  value: string;
}
