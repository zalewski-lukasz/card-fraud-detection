using AnomalyInfoConsumer.Models;
using AnomalyInfoConsumer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnomalyInfoConsumer.API.Controllers
{
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private readonly ILogger<AlertsController> _logger;
        private readonly IAlertRepository _alertRepository;

        public AlertsController(ILogger<AlertsController> logger,
            IAlertRepository alertRepository)
        {
            _logger = logger;
            _alertRepository = alertRepository;
        }

        [HttpGet]
        [Route("/api/get-all")]
        public IEnumerable<Alert> GetAll()
        {
            return _alertRepository.GetAllAlerts();
        }
    }
}
