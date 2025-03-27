import { render, screen, waitFor, act } from '@testing-library/react';
import { MemoryRouter, Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import { mockPositions, mockDepartments, mockStatuses, mockRecruiters } from '../data/mockdata';
import CreateEditPage from '../pages/CreateEditPage/CreateEditPage';


// Mock de axios para simular las solicitudes HTTP
jest.mock('axios');


jest.mock('../services/apiService', () => ({
    getDepartments: jest.fn(),
    getStatuses: jest.fn(),
    getRecruiters: jest.fn(),
}));


test('renders CreateEditPage correctly for creating new position', async () => {

    require('../services/apiService').getDepartments.mockResolvedValue(mockDepartments);
    require('../services/apiService').getStatuses.mockResolvedValue(mockStatuses);
    require('../services/apiService').getRecruiters.mockResolvedValue(mockRecruiters);

    render(
        <MemoryRouter initialEntries={['/create']}>
            <Routes>
                <Route path="/create" element={<CreateEditPage />} />
            </Routes>
        </MemoryRouter>
    );

    await waitFor(() => expect(screen.queryByText(/Loading.../i)).not.toBeInTheDocument());
    await waitFor(() => expect(screen.queryByText(/Error loading data/i)).not.toBeInTheDocument());

    expect(screen.getByText(/Create New Position/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Position Number/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Title/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Position Status/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Department/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Recruiter/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Budget/i)).toBeInTheDocument();
});