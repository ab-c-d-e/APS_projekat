export * from './auth.service';
import { AuthService } from './auth.service';
export * from './scientificPaper.service';
import { ScientificPaperService } from './scientificPaper.service';
export const APIS = [AuthService, ScientificPaperService];
