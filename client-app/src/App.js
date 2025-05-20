import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [message, setMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const handleTriggerTransform = async () => {
    setIsLoading(true);
    setMessage('');
    setError('');
    try {
      // The backend API is expected to be running on http://localhost:7000
      const response = await axios.post('http://localhost:7000/api/DataTransform/trigger');
      setMessage(`Success: ${response.data.message} (Job ID: ${response.data.jobId})`);
    } catch (err) {
      let errorMessage = 'Failed to trigger transformation.';
      if (err.response) {
        errorMessage += ` Server responded with ${err.response.status}: ${JSON.stringify(err.response.data)}`;
      } else if (err.request) {
        errorMessage += ' No response received from server. Is the backend running?';
      } else {
        errorMessage += ` Error: ${err.message}`;
      }
      setError(errorMessage);
      console.error("Error triggering transformation:", err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="App">
      <header className="App-header">
        <h1>Data Transformation Trigger</h1>
      </header>
      <main className="App-main">
        <div className="controls-container">
          <button 
            onClick={handleTriggerTransform} 
            disabled={isLoading}
            className="trigger-button"
          >
            {isLoading ? (
              <>
                <span className="spinner" />
                Triggering...
              </>
            ) : 'Trigger Manual Data Transformation'}
          </button>
        </div>
        
        {message && (
          <div className="message-container success-message">
            <p>{message}</p>
          </div>
        )}
        {error && (
          <div className="message-container error-message">
            <p>{error}</p>
          </div>
        )}
      </main>
    </div>
  );
}

export default App;
