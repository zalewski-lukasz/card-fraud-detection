using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;

namespace TransactionSimulator.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private IList<User> _users;

    public UserRepository()
    {
        _users = new List<User>();
    }

    public void AddUser(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException("User is null");
        }

        if (GetUser(user.Id) is not null)
        {
            throw new InvalidOperationException($"Transaction given by id {user.Id} already exists");
        }

        _users.Add(user);
        return;
    }

    public User GetUser(int id)
    {
        var user = _users.FirstOrDefault(t => t.Id == id);

        if (user is null)
        {
            throw new Exception($"Transaction given by id {id} does not exist");
        }

        return user;
    }

    public IList<int> GetUserIds()
    {
        if (_users == null)
        {
            return new List<int>();
        }

        return _users.Select(u => u.Id).ToList();
    }
}
