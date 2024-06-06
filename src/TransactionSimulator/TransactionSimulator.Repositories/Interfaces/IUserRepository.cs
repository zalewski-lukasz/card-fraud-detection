using TransactionSimulator.Models;

namespace TransactionSimulator.Repositories.Interfaces;

public interface IUserRepository
{
    void AddUser(User user);
    User GetUser(int id);
    IList<int> GetUserIds();
}
