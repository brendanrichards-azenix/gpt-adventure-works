import React from 'react';
import Layout from './components/Layout'
import { HashRouter as Router, Route, Routes, NavLink } from 'react-router-dom'
import './App.css';
import Home from './components/Home';
import Weather from './components/Weather';

function App() {
  return (
    <Layout>
      <Router>
        <div>
          <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
              <div className="navbar-nav ms-auto">
                <NavLink to="/" className="nav-item nav-link">Home</NavLink>
                <NavLink to="/weather" className="nav-item nav-link">Weather</NavLink>
              </div>
            </div>
          </nav>
          <Routes>
            <Route path="/" element={<Home/>} />
            <Route path="/weather" element={<Weather/>} />
          </Routes>
        </div>
      </Router>
    </Layout>
   
  );
}

export default App;
