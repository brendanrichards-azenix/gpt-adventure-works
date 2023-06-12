import React from 'react';
import { useEffect, useState } from 'react';


export interface Forecast {
    date: string,
    temperatureC: number,
    temperatureF: number,
    summary: string
}


function Weather() {

    const [forecasts, setForecasts] = useState<Forecast[]>([]);

    useEffect(() => {
        fetch('/api/weatherforecast')
            .then(response => response.json())
            .then((response: Forecast[]) => setForecasts(response));
    }, []);

    return (
      <div>
        <h1>Weather</h1>
        <p>Simple example fetching some data from the server.</p>
        <table className="table table-striped" aria-labelledby="tableLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
      </div>
    );
}


export default Weather;
  