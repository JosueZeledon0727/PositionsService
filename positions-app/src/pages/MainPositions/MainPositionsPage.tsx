import React, { useEffect, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import { Position } from '../../types/Types'; // Importamos la interfaz Position
import './MainPositionsPage.css';
import axios from 'axios';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { useApiData } from '../../hooks/useApiData';

const MainPositionsPage: React.FC = () => {
    const [positions, setPositions] = useState<Position[]>([]);
    const [filter, setFilter] = useState<string>('');
    const [selectedDepartment, setSelectedDepartment] = useState<string>(''); // Department Filter
    const [selectedStatus, setSelectedStatus] = useState<string>(''); // Statuses Filter
    const [error, setError] = useState<string>('');

    const { departments, statuses, loading: apiLoading, error: apiError } = useApiData();

    // API URL
    const apiUrl = process.env.REACT_APP_API_URL || 'http://localhost:5054';  // Fallback to localhost if not defined

    const connectionRef = useRef<HubConnection | null>(null);

    // To check if component is still mounted
    const isMounted = useRef<boolean>(true);

    // Initial data load (on mounting)
    useEffect(() => {

        isMounted.current = true;

        // Get Positions
        const fetchPositions = async () => {
            try {
                const response = await axios.get(`${apiUrl}/api/positions`);
                setPositions(response.data);
            } catch (error) {
                setError('Error on loading positions');
            }
        };

        // Calling the fetchPositions endpoint
        fetchPositions();

        const startConnection = async () => {
            console.log("Start Connection");
            if (isMounted.current && !connectionRef.current) {
                connectionRef.current = new HubConnectionBuilder()
                    .withUrl(`${apiUrl}/positionHub`)
                    .build();

                try {
                    // Verify state connection
                    if (connectionRef.current.state === HubConnectionState.Disconnected) {
                        await connectionRef.current.start();
                        console.log('Connected to SignalR');
                    } else {
                        console.log(`Connection already on state: ${connectionRef.current.state}`);
                    }
                } catch (err) {
                    console.error('Error connecting to SignalR:', err);
                    setError('Error connecting to SignalR. Retrying...');
                }
            }
        };

        // Start connection and load data
        startConnection();

        // Handling updates on SignalR
        connectionRef.current?.on('PositionUpdated', (position: string) => {
            if (isMounted.current) {
                console.log(`Message: ${position}`);
                fetchPositions(); // Reload the Positions based on message received
            }
        });

        // Handling disconnection
        connectionRef.current?.onclose((error) => {
            if (isMounted.current) {
                console.error('Conexión de SignalR cerrada con error:', error);
                setTimeout(() => {
                    if (isMounted.current) {
                        startConnection();
                    }
                }, 5000);
            }
        });

        // Close connection when unmounting component
        return () => {
            isMounted.current = false;
            if (connectionRef.current) {
                connectionRef.current.stop().then(() => {
                    console.log('SignalR connection closed.');
                }).catch((err) => {
                    console.error('Error on closing SignalR connection:', err);
                });
                // Deleting connection reference
                connectionRef.current = null;
            }
        };


    }, [apiUrl]);

    // Filtering positions based on title or position number
    const filteredPositions = positions.filter((position) => {
        const matchesFilter =
            position.positionNumber.includes(filter) ||
            position.title.toLowerCase().includes(filter.toLowerCase());

        const matchesDepartment =
            selectedDepartment === '' || position.department.departmentName === selectedDepartment;

        const matchesStatus =
            selectedStatus === '' || position.status.statusName === selectedStatus;

        return matchesFilter && matchesDepartment && matchesStatus;
    });

    const handleDelete = async (positionID: number) => {
        try {
            const response = await axios.delete(`${apiUrl}/api/positions/${positionID}`);
            if (response.status === 204) {
                setPositions(positions.filter((position) => position.positionID !== positionID));
            }
        } catch (error) {
            setError('Error on deleting position');
        }
    }

    // In case there's an error, we'll return the error message instead of the components
    if (error) {
        return <div>{error}</div>;
    }

    if (apiLoading) return <div>Loading...</div>;
    if (apiError) return <div>Error loading the data: {apiError}</div>;

    return (
        <div className='list'>
            <h1 className='main-title'>Positions Management</h1>

            {/* Search */}
            <input
                type="text"
                placeholder="Search by Position Number or Title"
                value={filter}
                onChange={(e) => setFilter(e.target.value)}
            />
            <select
                value={selectedDepartment}
                onChange={(e) => setSelectedDepartment(e.target.value)}
            >
                <option value="">All Departments</option>
                {departments.map((department) => (
                    <option key={department.departmentID} value={department.departmentName}>
                        {department.departmentName}
                    </option>
                ))}
            </select>

            <select
                value={selectedStatus}
                onChange={(e) => setSelectedStatus(e.target.value)}
            >
                <option value="">All Statuses</option>
                {statuses.map((status) => (
                    <option key={status.positionStatusID} value={status.statusName}>
                        {status.statusName}
                    </option>
                ))}
            </select>

            {/* Positions table */}
            <table className='table'>
                <thead>
                    <tr>
                        <th>Position Number</th>
                        <th>Title</th>
                        <th>Status</th>
                        <th>Department</th>
                        <th>Recruiter</th>
                        <th>Budget</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredPositions.map((position) => (
                        <tr key={position.positionID}>
                            <td>{position.positionNumber}</td>
                            <td>{position.title}</td>
                            <td>{position.status.statusName}</td>
                            <td>{position.department.departmentName}</td>
                            <td>{position.recruiter.recruiterName}</td>
                            <td>${position.budget.toLocaleString()}</td>
                            <td>
                                <Link className="linkButton" to={`/edit/${position.positionID}`}>Edit</Link>&nbsp;&nbsp;|
                                <button className="button deleteButton" onClick={() => handleDelete(position.positionID)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {/* Button for creating position */}
            <Link className="linkButton createButton" to="/create">Create New Position</Link>
        </div>
    );
};

export default MainPositionsPage;