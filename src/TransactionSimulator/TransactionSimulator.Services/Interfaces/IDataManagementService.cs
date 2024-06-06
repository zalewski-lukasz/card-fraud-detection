using TransactionSimulator.Models;

namespace TransactionSimulator.Services.Interfaces;

public interface IDataManagementService
{
    IList<User> GetRandomUsers(int count);
}
