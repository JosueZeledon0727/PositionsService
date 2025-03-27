import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import MainPositionsPage from '../pages/MainPositions/MainPositionsPage';

jest.mock('../data/mockdata', () => ({
    mockPositions: [
        {
            PositionID: 1,
            PositionNumber: '12345',
            Title: 'Software Engineer',
            Budget: 50000,
            Status: { PositionStatusID: 1, StatusName: 'Active' },
            Department: { DepartmentID: 1, DepartmentName: 'Engineering' },
            Recruiter: { RecruiterID: 1, RecruiterName: 'John Doe' }
        },
    ],
    mockDepartments: [
        { DepartmentID: 1, DepartmentName: 'Engineering' },
        { DepartmentID: 2, DepartmentName: 'HR' },
    ],
    mockStatuses: [
        { PositionStatusID: 1, StatusName: 'Active' },
        { PositionStatusID: 2, StatusName: 'Inactive' },
    ]
}));

test('renders MainPositionsPage correctly and shows positions', () => {
    render(
        <Router>
            <MainPositionsPage />
        </Router>
    );

    // Verify main title is present
    expect(screen.getByText(/Positions Management/i)).toBeInTheDocument();

    // Verify table rendering
    expect(screen.getByText('Software Engineer')).toBeInTheDocument();
    expect(screen.getByText('12345')).toBeInTheDocument();
    expect(screen.getByText('Active')).toBeInTheDocument();

    // Verify filters are present and rendered
    expect(screen.getByPlaceholderText(/Search by Position Number or Title/i)).toBeInTheDocument();
    expect(screen.getByText('All Departments')).toBeInTheDocument();
    expect(screen.getByText('All Statuses')).toBeInTheDocument();
});

test('clicking "Create New Position" navigates to the create page', () => {
    render(
        <Router>
            <MainPositionsPage />
        </Router>
    );

    const createButton = screen.getByText(/Create New Position/i);
    expect(createButton).toBeInTheDocument();

    fireEvent.click(createButton);

    expect(window.location.pathname).toBe('/create');
});