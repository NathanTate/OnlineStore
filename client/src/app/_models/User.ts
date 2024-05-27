export interface User {
    tokenExperationDate: Date;
    email: string;
    firstName: string;
    lastName: string;
    token: string | null;
    roles: string[];
}