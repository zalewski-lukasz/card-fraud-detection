import React, { useState } from 'react';
import './componentstyle.css';

const AlertTable = ({data}) => {

  const [currentPage, setCurrentPage] = useState(0);
  const recordsPerPage = 5;

  const handlePrev = () => {
    if (currentPage > 0) setCurrentPage(currentPage - 1);
  };

  const handleNext = () => {
    if ((currentPage + 1) * recordsPerPage < data.length) setCurrentPage(currentPage + 1);
  };

  const startIdx = currentPage * recordsPerPage;
  const endIdx = startIdx + recordsPerPage;
  const currentRecords = data.slice(startIdx, endIdx);

  return (
    <div className="flex flex-col items-center justify-center w-[800px]">
      <table className="min-w-full bg-gray-800 text-white">
        <thead>
          <tr>
            <th className="py-2 px-4">Name</th>
            <th className="py-2 px-4">UV</th>
            <th className="py-2 px-4">PV</th>
          </tr>
        </thead>
        <tbody>
          {currentRecords.map((record, index) => (
            <tr key={index} className="border-t border-gray-700">
              <td className="py-2 px-4">{record.name}</td>
              <td className="py-2 px-4">{record.uv}</td>
              <td className="py-2 px-4">{record.pv}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className="flex justify-between w-full mt-4">
        <button
          onClick={handlePrev}
          disabled={currentPage === 0}
          className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded disabled:opacity-50"
        >
          Previous
        </button>
        <button
          onClick={handleNext}
          disabled={(currentPage + 1) * recordsPerPage >= data.length}
          className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
}

export default AlertTable;
