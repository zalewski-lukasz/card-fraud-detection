using Microsoft.AspNetCore.Mvc;
using TransactionSimulator.Models;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.API.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataManagementService _dataManagementService;
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger,
            IDataManagementService dataManagementService)
        {
            _logger = logger;
            _dataManagementService = dataManagementService;
        }

        [HttpGet]
        [Route("/api/get-random-users")]
        public IEnumerable<User> GetUsers(int count)
        {
            return _dataManagementService.GetRandomUsers(count);
        }
    }
}
