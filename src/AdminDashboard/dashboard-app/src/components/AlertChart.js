import React from "react";
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import './componentstyle.css';

const AlertChart = ({data}) => {

    return (
        <div className=" bg-gray-900">

        <LineChart
          width={800}
          height={600}
          data={data}
          margin={{
            top: 5,
            right: 30,
            left: 20,
            bottom: 5,
          }}
          className="text-white"
        >
          <CartesianGrid strokeDasharray="3 3" stroke="#444" />
          <XAxis dataKey="time" stroke="#ccc" />
          <YAxis stroke="#ccc" />
          <Tooltip contentStyle={{ backgroundColor: '#333', borderColor: '#444' }} itemStyle={{ color: '#fff' }} />
          <Legend wrapperStyle={{ color: '#ccc' }} />
          <Line type="monotone" dataKey="Value of the transaction extends the limit!" stroke="#8884d8" activeDot={{ r: 8 }} />
          <Line type="monotone" dataKey="High number of transactions detected for the user within a minute!" stroke="#82ca9d" activeDot={{ r: 8 }} />
          <Line type="monotone" dataKey="Too big of a change in location in time window!" stroke="#ff00ff" activeDot={{ r: 8 }}/>
        </LineChart>
      </div>
        
    )

}

export default AlertChart;