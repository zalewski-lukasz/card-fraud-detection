using AnomalyInfoConsumer.Models;
using AnomalyInfoConsumer.Repositories.Interfaces;

namespace AnomalyInfoConsumer.Repositories.Implementations;

public class AlertRepository : IAlertRepository
{
    private IList<Alert> _alerts;

    public AlertRepository()
    {
        _alerts = new List<Alert>();
    }

    public void AddAlert(Alert alert)
    {
        _alerts.Add(alert);
    }

    public IEnumerable<Alert> GetAllAlerts()
    {
        return _alerts;
    }
}
