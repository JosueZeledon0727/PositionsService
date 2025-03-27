import React, { useEffect, useState } from 'react';
import { mockDepartments, mockPositions, mockRecruiters, mockStatuses } from '../../data/mockdata';
import { Department, Position, PositionCreateDto, PositionStatus, Recruiter } from '../../types/Types';
import { useNavigate, useParams } from 'react-router-dom';
import './CreateEditPage.css';
import axios from 'axios';

const CreateEditPage: React.FC = () => {
    const [position, setPosition] = useState<PositionCreateDto>({
        positionNumber: '',
        title: '',
        positionStatusID: 1,
        departmentID: 1,
        recruiterID: 1,
        budget: 0
    });

    const [departments, setDepartments] = useState<Department[]>([]);
    const [statuses, setStatuses] = useState<PositionStatus[]>([]);
    const [recruiters, setRecruiters] = useState<Recruiter[]>([]);
    const [isEditMode, setIsEditMode] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>('');
    const { positionId } = useParams<{ positionId: string }>(); // Getting the Position ID
    const navigate = useNavigate();

    // Loading position data if we are editing
    useEffect(() => {
        const fetchDepartments = async () => {
            try {
                const response = await axios.get('http://localhost:5054/api/departments');
                setDepartments(response.data);
            } catch (error) {
                setError('Error al cargar los departamentos.');
            }
        };

        const fetchStatuses = async () => {
            try {
                const response = await axios.get('http://localhost:5054/api/positionstatuses');
                setStatuses(response.data);
            } catch (error) {
                setError('Error al cargar los estados.');
            }
        };

        const fetchRecruiters = async () => {
            try {
                const response = await axios.get('http://localhost:5054/api/recruiters');
                setRecruiters(response.data);
            } catch (error) {
                setError('Error al cargar los reclutadores.');
            }
        };

        fetchDepartments();
        fetchStatuses();
        fetchRecruiters();

        if (positionId) {
            setIsEditMode(true);
            const fetchPosition = async () => {
                try {
                    const response = await axios.get(`http://localhost:5054/api/positions/${positionId}`);
                    setPosition({
                        positionNumber: response.data.positionNumber,
                        title: response.data.title,
                        positionStatusID: response.data.status.positionStatusID,
                        departmentID: response.data.department.departmentID,
                        recruiterID: response.data.recruiter.recruiterID,
                        budget: response.data.budget
                    });
                } catch (error) {
                    setError('Position not found');
                    console.log(error);
                }
            };
            fetchPosition();
        }
    }, [positionId]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        setPosition((prevPosition) => ({
            ...prevPosition,
            [name]: value
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);

        try {
            if (isEditMode) {
                // Update the position
                const response = await axios.put(`http://localhost:5054/api/positions/${positionId}`, {
                    ...position,
                    positionStatusID: position.positionStatusID,
                    departmentID: position.departmentID,
                    recruiterID: position.recruiterID
                });
                if (response.status === 204) {
                    navigate('/');
                }
            } else {
                // Creating a new position
                const response = await axios.post('http://localhost:5054/api/positions', position);
                if (response.status === 201) {
                    navigate('/');
                }
            }
        } catch (error) {
            setLoading(false);
            setError('Error on saving position data.');
        }
    };

    // Redirect to main page (MainPositionsPage)
    const handleBack = () => {
        navigate('/');
    };


    return (
        <div className='creation-edit-page'>
            <button className="back-button" onClick={handleBack}>
                Back to Positions List
            </button>
            <h1>{isEditMode ? 'Edit Position' : 'Create New Position'}</h1>

            {error && <div className="error-message">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label>Position Number</label>
                    <input
                        type="text"
                        name="positionNumber"
                        value={position.positionNumber}
                        onChange={handleChange}
                        required
                        disabled={isEditMode} // No permitir modificar el número de posición en el modo de edición
                    />
                </div>

                <div className="form-group">
                    <label>Title</label>
                    <input
                        type="text"
                        name="title"
                        value={position.title}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label>Position Status</label>
                    <select
                        name="positionStatusID"
                        value={position.positionStatusID}
                        onChange={handleChange}
                        required
                    >
                        {statuses.map(status => (
                            <option key={status.positionStatusID} value={status.positionStatusID}>
                                {status.statusName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Department</label>
                    <select
                        name="departmentID"
                        value={position.departmentID}
                        onChange={handleChange}
                        required
                    >
                        {departments.map(department => (
                            <option key={department.departmentID} value={department.departmentID}>
                                {department.departmentName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Recruiter</label>
                    <select
                        name="recruiterID"
                        value={position.recruiterID}
                        onChange={handleChange}
                        required
                    >
                        {recruiters.map(recruiter => (
                            <option key={recruiter.recruiterID} value={recruiter.recruiterID}>
                                {recruiter.recruiterName}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="form-group">
                    <label>Budget</label>
                    <input
                        type="number"
                        name="budget"
                        value={position.budget}
                        onChange={handleChange}
                        required
                        min="0"
                    />
                </div>

                <button type="submit" disabled={loading}>
                    {isEditMode ? 'Update Position' : 'Create Position'}
                </button>
            </form>
        </div>
    );
};

export default CreateEditPage;