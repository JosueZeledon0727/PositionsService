import React, { useEffect, useState } from 'react';
import { PositionCreateDto } from '../../types/Types';
import { useNavigate, useParams } from 'react-router-dom';
import './CreateEditPage.css';
import axios from 'axios';
import { useApiData } from '../../hooks/useApiData';
import { toast } from 'react-toastify';

const CreateEditPage: React.FC = () => {
    const [position, setPosition] = useState<PositionCreateDto>({
        positionNumber: '',
        title: '',
        positionStatusID: 1,
        departmentID: 1,
        recruiterID: 1,
        budget: 0
    });
    const [isEditMode, setIsEditMode] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string>('');
    const { positionId } = useParams<{ positionId: string }>(); // Getting the Position ID
    const navigate = useNavigate();

    const { departments, statuses, recruiters, loading: apiLoading, error: apiError } = useApiData();

    // Loading position data if we are editing
    useEffect(() => {

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
                    toast.success('Position updated successfully!');
                    navigate('/');
                }
            } else {
                // Creating a new position
                const response = await axios.post('http://localhost:5054/api/positions', position);
                if (response.status === 201) {
                    toast.success('Position created successfully!');
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


    // Handling loading and errors from the API
    if (apiLoading) return <div>Loading...</div>;
    if (apiError) return <div>{apiError}</div>;

    return (
        <div className='creation-edit-page'>
            <button className="back-button" onClick={handleBack}>
                Back to Positions List
            </button>
            <h1>{isEditMode ? 'Edit Position' : 'Create New Position'}</h1>

            {error && <div className="error-message">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="positionNumber">Position Number</label>
                    <input
                        id="positionNumber"
                        type="text"
                        name="positionNumber"
                        value={position.positionNumber}
                        onChange={handleChange}
                        required
                        disabled={isEditMode} // Not allowing to update the Position Number on Edit Mode
                    />
                </div>

                <div className="form-group">
                    <label htmlFor='title'>Title</label>
                    <input
                        id="title"
                        type="text"
                        name="title"
                        value={position.title}
                        onChange={handleChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="positionStatusID">Position Status</label>
                    <select
                        id="positionStatusID"
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
                    <label htmlFor='departmentID'>Department</label>
                    <select
                        id="departmentID"
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
                    <label htmlFor='recruiterID'>Recruiter</label>
                    <select
                        id="recruiterID"
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
                    <label htmlFor='budget'>Budget</label>
                    <input
                        id="budget"
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