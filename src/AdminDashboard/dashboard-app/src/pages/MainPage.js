import React, { useState, useEffect } from 'react';
import './pagestyle.css';

import AlertChart from "../components/AlertChart";
import Heading from '../components/Heading';
import AlertTable from '../components/AlertTable';

const MainPage = () => {

    const [alertData, setAlertData] = useState([]);
    const [reasonCounts, setReasonCounts] = useState({});

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
                console.log("Data fetched and processed");
            } catch (error) {
                console.error("Error fetching data:", error);
            }
        };

        fetchData();
        const interval = setInterval(fetchData, 10000); // 10000 ms = 10 seconds
        return () => clearInterval(interval);
    }, []);

    const data = [
        {
          name: 'Page A',
          uv: 4000,
          pv: 2400,
          amt: 2400,
        },
        {
          name: 'Page B',
          uv: 3000,
          pv: 1398,
          amt: 2210,
        },
        {
          name: 'Page C',
          uv: 2000,
          pv: 9800,
          amt: 2290,
        },
        {
          name: 'Page D',
          uv: 2780,
          pv: 3908,
          amt: 2000,
        },
        {
          name: 'Page E',
          uv: 1890,
          pv: 4800,
          amt: 2181,
        },
        {
          name: 'Page F',
          uv: 2390,
          pv: 3800,
          amt: 2500,
        },
        {
          name: 'Page G',
          uv: 3490,
          pv: 4300,
          amt: 2100,
        },
      ];

    return (
        <div className="flex flex-col items-center min-h-screen bg-gray-900">
            <Heading value={"Anomaly detection dashoard"}/>
            <AlertChart data={data}></AlertChart>
            <Heading value={"Detected anomalies"}/>
            <AlertTable data={alertData} />
        </div>       
    )
}

export default MainPage;