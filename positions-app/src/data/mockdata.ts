// mockData.ts
import { Position, PositionStatus, Department, Recruiter } from '../types/Types';

export const mockPositions: Position[] = [
    {
        positionID: 1,
        positionNumber: '12345',
        title: 'Software Engineer',
        positionStatusID: 1,
        departmentID: 1,
        recruiterID: 1,
        budget: 100000,
        status: { positionStatusID: 1, statusName: 'Active' },
        department: { departmentID: 1, departmentName: 'Engineering' },
        recruiter: { recruiterID: 1, recruiterName: 'John Doe' },
    },
    {
        positionID: 2,
        positionNumber: '54321',
        title: 'Product Manager',
        positionStatusID: 2,
        departmentID: 2,
        recruiterID: 2,
        budget: 120000,
        status: { positionStatusID: 2, statusName: 'Inactive' },
        department: { departmentID: 2, departmentName: 'Product' },
        recruiter: { recruiterID: 2, recruiterName: 'Jane Smith' },
    },

];


export const mockDepartments: Department[] = [
    { departmentID: 1, departmentName: 'Engineering' },
    { departmentID: 2, departmentName: 'Sales' },
    { departmentID: 3, departmentName: 'HR' },
    { departmentID: 4, departmentName: 'Marketing' },
];

export const mockRecruiters: Recruiter[] = [
    { recruiterID: 1, recruiterName: 'Alice' },
    { recruiterID: 2, recruiterName: 'Bob' },
    { recruiterID: 3, recruiterName: 'Charlie' },
    { recruiterID: 4, recruiterName: 'David' },
];

export const mockStatuses: PositionStatus[] = [
    { positionStatusID: 1, statusName: 'Active' },
    { positionStatusID: 2, statusName: 'Inactive' },
    { positionStatusID: 3, statusName: 'Closed' }
];