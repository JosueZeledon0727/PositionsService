export interface PositionStatus {
    positionStatusID: number;
    statusName: string;
}

export interface Department {
    departmentID: number;
    departmentName: string;
}

export interface Recruiter {
    recruiterID: number;
    recruiterName: string;
}

export interface Position {
    positionID: number;
    positionNumber: string;
    title: string;
    positionStatusID: number;
    departmentID: number;
    recruiterID: number;
    budget: number;
    status: PositionStatus;
    department: Department;
    recruiter: Recruiter;
}

export interface PositionCreateDto {
    positionNumber: string;
    title: string;
    positionStatusID: number;
    departmentID: number;
    recruiterID: number;
    budget: number;
}