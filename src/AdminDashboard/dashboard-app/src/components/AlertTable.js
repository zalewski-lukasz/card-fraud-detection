import React, { useState } from 'react';
import './componentstyle.css';

const AlertTable = ({ data }) => {
  const [currentPage, setCurrentPage] = useState(0);
  const [filterReason, setFilterReason] = useState('');
  const recordsPerPage = 5;

  const handlePrev = () => {
    if (currentPage > 0) setCurrentPage(currentPage - 1);
  };

  const handleNext = () => {
    if ((currentPage + 1) * recordsPerPage < filteredData.length) setCurrentPage(currentPage + 1);
  };

  const handleFilterChange = (event) => {
    setFilterReason(event.target.value);
    setCurrentPage(0); // Reset to first page when filter changes
  };

  const filteredData = filterReason
    ? data.filter(record => record.reason === filterReason)
    : data;

  const startIdx = currentPage * recordsPerPage;
  const endIdx = startIdx + recordsPerPage;
  const currentRecords = filteredData.slice(startIdx, endIdx);

  return (
    <div className="flex flex-col items-center justify-center w-[800px] mb-6">
      <div className="flex justify-between w-full mb-4">
        <button
          onClick={handlePrev}
          disabled={currentPage === 0}
          className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded disabled:opacity-50"
        >
          Previous
        </button>
        <select
          value={filterReason}
          onChange={handleFilterChange}
          className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded"
        >
          <option value="">All Reasons</option>
          <option value="Value of the transaction extends the limit!">Value of the transaction extends the limit!</option>
          <option value="High number of transactions detected for the user within a minute!">High number of transactions detected for the user within a minute!</option>
          <option value="Too big of a change in location in time window!">Too big of a change in location in time window!</option>
          <option value="General outlier transaction detected!">General outlier transaction detected!</option>
        </select>
        <button
          onClick={handleNext}
          disabled={(currentPage + 1) * recordsPerPage >= filteredData.length}
          className="bg-gray-600 hover:bg-gray-500 text-white font-bold py-2 px-4 rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
      <table className="min-w-full bg-gray-800 text-white">
        <thead>
          <tr>
            <th className="py-2 px-4">Id</th>
            <th className="py-2 px-4">CardId</th>
            <th className="py-2 px-4">UserId</th>
            <th className="py-2 px-4">Reason</th>
            <th className="py-2 px-4">Timestamp</th>
          </tr>
        </thead>
        <tbody>
          {currentRecords.map((record, index) => (
            <tr key={index} className="border-t border-gray-700">
              <td className="py-2 px-4">{record.id}</td>
              <td className="py-2 px-4">{record.cardId}</td>
              <td className="py-2 px-4">{record.userId}</td>
              <td className="py-2 px-4">{record.reason}</td>
              <td className="py-2 px-4">{record.timestamp}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default AlertTable;