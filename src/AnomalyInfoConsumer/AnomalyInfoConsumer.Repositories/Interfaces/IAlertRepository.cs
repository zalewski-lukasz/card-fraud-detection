using AnomalyInfoConsumer.Models;

namespace AnomalyInfoConsumer.Repositories.Interfaces;

public interface IAlertRepository
{
    IEnumerable<Alert> GetAllAlerts();
    void AddAlert(Alert alert);
}
