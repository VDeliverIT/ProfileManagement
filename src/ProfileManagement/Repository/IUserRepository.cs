using ProfileManagement.Models;
using System.Collections.Generic;

namespace ProfileManagement.Repository
{
    public interface IUserRepository
    {
        void Add(User aUser);
        IEnumerable<User> GetAllUsers();
        User Find(string userName);
        User Remove(string userName);
        void Update(User aUser);
    }
}
