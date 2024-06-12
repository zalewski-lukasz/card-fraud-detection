using Microsoft.AspNetCore.Mvc;
using TransactionSimulator.BackgroundWorkers;
using TransactionSimulator.Models;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.API.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataManagementService _dataManagementService;
        private readonly DataProducerService _producerService;
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger,
            IDataManagementService dataManagementService,
            DataProducerService dataProducerService)
        {
            _logger = logger;
            _dataManagementService = dataManagementService;
            _producerService = dataProducerService;
        }

        [HttpGet]
        [Route("/api/get-random-users")]
        public IEnumerable<User> GetUsers(int count)
        {
            return _dataManagementService.GetRandomUsers(count);
        }

        [HttpPost]
        [Route("/api/generate-over-the-limit-anomaly")]
        public IActionResult GenerateOverTheLimitAnomaly()
        {
            _producerService.GenerateAnomalyTransactions("OVER_THE_LIMIT_ANOMALY");
            return Ok();
        }

        [HttpPost]
        [Route("/api/generate-multiple-transactions-anomaly")]
        public IActionResult GenerateMultipleTransactionAnomaly()
        {
            _producerService.GenerateAnomalyTransactions("MULTIPLE_TRANSACTIONS_ANOMALY");
            return Ok();
        }
    }
}
