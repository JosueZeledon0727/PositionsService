import { useEffect, useState } from 'react';
import { getDepartments, getStatuses, getRecruiters } from '../services/apiService';
import { Department, PositionStatus, Recruiter } from '../types/Types';

export const useApiData = () => {
    const [departments, setDepartments] = useState<Department[]>([]);
    const [statuses, setStatuses] = useState<PositionStatus[]>([]);
    const [recruiters, setRecruiters] = useState<Recruiter[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string>('');

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const departmentsData = await getDepartments();
                const statusesData = await getStatuses();
                const recruitersData = await getRecruiters();

                setDepartments(departmentsData);
                setStatuses(statusesData);
                setRecruiters(recruitersData);
            } catch (err) {
                setError('Error loading data');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    return { departments, statuses, recruiters, loading, error };
};