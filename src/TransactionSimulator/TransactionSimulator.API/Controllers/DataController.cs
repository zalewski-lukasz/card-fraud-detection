using Microsoft.AspNetCore.Mvc;
using TransactionSimulator.BackgroundWorkers;
using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.API.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataManagementService _dataManagementService;
        private readonly DataProducerService _producerService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger,
            IDataManagementService dataManagementService,
            DataProducerService dataProducerService,
            ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _dataManagementService = dataManagementService;
            _producerService = dataProducerService;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
        [Route("/api/users/random")]
        public IEnumerable<User> GetUsers(int count)
        {
            return _dataManagementService.GetRandomUsers(count);
        }

        [HttpGet]
        [Route("/api/transactions")]
        public IEnumerable<Transaction> GetTransactions(int userId, int cardId)
        {
            return _transactionRepository.GetTransactionsBuUserAndCard(cardId, userId);
        }

        [HttpPost]
        [Route("/api/anomalies/over-the-limit")]
        public IActionResult GenerateOverTheLimitAnomaly()
        {
            _producerService.GenerateAnomalyTransactions("OVER_THE_LIMIT_ANOMALY");
            return Ok();
        }

        [HttpPost]
        [Route("/api/anomalies/multiple-transactions")]
        public IActionResult GenerateMultipleTransactionAnomaly()
        {
            _producerService.GenerateAnomalyTransactions("MULTIPLE_TRANSACTIONS_ANOMALY");
            return Ok();
        }

        [HttpPost]
        [Route("/api/anomalies/location-change")]
        public IActionResult GenerateLocationAnomaly()
        {
            _producerService.GenerateAnomalyTransactions("LOCATION_ANOMALY");
            return Ok();
        }
    }
}
