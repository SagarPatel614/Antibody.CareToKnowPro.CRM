import { ISpecialty as Specialty } from './specialty';
import { IUser as User } from './user';
import { Subscription } from 'rxjs';

export interface IDuplicateCheckResponse {
  existingRecords?: Array<IDuplicateCheckRecord>;
  notExistingRecords?: Array<IDuplicateCheckRecord>;
}

export interface IDuplicateCheckRecord {
  firstName?: string;
  lastName?: string;
  email?: string;
  provinceCode ?: string;
  specialties?: string;
  graduationYear?: number;
  language?: string;
}

export class FileUploadModel {
  data: File;
  state: string;
  inProgress: boolean;
  progress: number;
  canRetry: boolean;
  canCancel: boolean;
  sub?: Subscription;
}
