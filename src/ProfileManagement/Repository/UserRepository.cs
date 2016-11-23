using System.Collections.Generic;
using ProfileManagement.Models;
using System.Collections.Concurrent;

namespace ProfileManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private static ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
        public UserRepository()
        {
            Add(new User { Id = 1, FirstName = "girijaa", LastName = "doraiswamy", UserName = "girijaad", Password = "123456", Email = "yali.girijaa@gmail.com", Role = "shopowner" });
            Add(new User { Id = 2, FirstName = "test", LastName = "driver", UserName = "driver1", Password = "123456", Email = "yali.girijaa@gmail.com", Role = "driver" });
            Add(new User { Id = 3, FirstName = "test", LastName = "customer", UserName = "customer1", Password = "123456", Email = "yali.girijaa@gmail.com", Role = "customer" });
        }
        public void Add(User aUser)
        {
            Users[aUser.UserName] = aUser;
        }

        public User Find(string userName)
        {
            User aUser;
            Users.TryGetValue(userName, out aUser);
            return aUser;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return Users.Values;
        }

        public User Remove(string userName)
        {
            User aUser;
            Users.TryRemove(userName, out aUser);
            return aUser;
        }

        public void Update(User aUser)
        {
            Users[aUser.UserName] = aUser;
        }
    }
}
