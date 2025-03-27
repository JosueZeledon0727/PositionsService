import React from 'react';
import logo from './logo.svg';
import MainPositionsPage from './pages/MainPositions/MainPositionsPage';
import CreateEditPage from './pages/CreateEditPage/CreateEditPage';
import { Routes, Route } from 'react-router-dom';
import './App.css';

function App() {
  return (
    <div className="App">
      <Routes>
        <Route path="/" element={<MainPositionsPage />} />
        <Route path="/create" element={<CreateEditPage />} />
        <Route path="/edit/:positionId" element={<CreateEditPage />} />
      </Routes>
    </div>
  );
}

export default App;
