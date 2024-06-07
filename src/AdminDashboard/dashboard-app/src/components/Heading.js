import React from "react";
import './componentstyle.css';

const Heading = ({value}) => {

    return (
        <div className="w-[800px]">
            <h1 className="text-white text-2xl lg:text-4xl font-bold font-sans m-6">{value}</h1>
        </div>
    )
}

export default Heading;