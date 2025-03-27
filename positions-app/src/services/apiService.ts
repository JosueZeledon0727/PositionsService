import axios from 'axios';
import { Department, PositionStatus, Recruiter } from '../types/Types';

const apiUrl = process.env.REACT_APP_API_URL || 'http://localhost:5054';

export const getDepartments = async (): Promise<Department[]> => {
    const response = await axios.get(`${apiUrl}/api/departments`);
    return response.data;
};

export const getStatuses = async (): Promise<PositionStatus[]> => {
    const response = await axios.get(`${apiUrl}/api/positionstatuses`);
    return response.data;
};

export const getRecruiters = async (): Promise<Recruiter[]> => {
    const response = await axios.get(`${apiUrl}/api/recruiters`);
    return response.data;
};