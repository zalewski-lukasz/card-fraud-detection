import React, { useState, useEffect } from 'react';
import './pagestyle.css';

import AlertChart from "../components/AlertChart";
import Heading from '../components/Heading';
import AlertTable from '../components/AlertTable';

const MainPage = () => {
    const [alertData, setAlertData] = useState([]);
    const [reasonCounts, setReasonCounts] = useState({});
    const [chartData, setChartData] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('http://localhost:5261/api/get-all');
                const data = await response.json();

                const reasonMap = data.reduce((acc, alert) => {
                    acc[alert.reason] = (acc[alert.reason] || 0) + 1;
                    return acc;
                }, {});

                setAlertData(data);
                setReasonCounts(reasonMap);
                setChartData(transformChartData(data));
                console.log("Data fetched and processed");
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };

        fetchData();
        const interval = setInterval(fetchData, 10000); // 10000 ms = 10 seconds
        return () => clearInterval(interval);
    }, []);

    const transformChartData = (data) => {
        const groupedData = data.reduce((acc, alert) => {
            const date = new Date(alert.timestamp);
            const time = `${date.getHours()}:${date.getMinutes()}`;
            const reason = alert.reason;
            console.log(reason)
            if (!acc[time]) {
                acc[time] = {};
            }

            if (!acc[time][reason]) {
                acc[time][reason] = 0;
            }

            acc[time][reason] += 1;
            return acc;
        }, {});

        const chartData = Object.keys(groupedData).map(time => {
            return {
                time,
                ...groupedData[time]
            };
        });

        return chartData;
    };

    return (
        <div className="flex flex-col items-center min-h-screen bg-gray-900">
            <Heading value={"Anomaly detection dashboard"} />
            <AlertChart data={chartData} />
            <Heading value={"Detected anomalies"} />
            <AlertTable data={alertData} />
        </div>
    )
}

export default MainPage;
