using TransactionSimulator.Models;
using TransactionSimulator.Repositories.Interfaces;
using TransactionSimulator.Services.Interfaces;

namespace TransactionSimulator.Services.Implementations;

public class DataManagementService : IDataManagementService
{
    private readonly IUserRepository _userRepository;

    public DataManagementService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IList<User> GetRandomUsers(int count)
    {
        var users = new List<User>();
        var rand = new Random();

        for (int i = 0; i < count; i++)
        {
            users.Add(_userRepository.GetUser(rand.Next(0, count)));
        }

        return users;
    }
}
