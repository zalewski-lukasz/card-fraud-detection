import React from "react";
import './componentstyle.css';

const Heading = ({value}) => {

    return (
        <div>
            <h1 className="text-white text-2xl lg:text-4xl font-bold font-sans m-5">{value}</h1>
        </div>
    )
}

export default Heading;