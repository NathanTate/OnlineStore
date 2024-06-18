export interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    token: string | null;
    tokenExperationDate: Date;
    roles: string[];
}