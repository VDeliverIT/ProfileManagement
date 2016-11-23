using Microsoft.Extensions.Primitives;
using ProfileManagement.Models;
using ProfileManagement.Repository;
using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ProfileManagement.Utilities
{
    public class UserUtilities
    {
        private static ConcurrentDictionary<string, UserToken> UserTokens = new ConcurrentDictionary<string, UserToken>();
        private static List<string> Secrets;
        static UserUtilities()
        {
            Secrets = new List<string>();
            Secrets.Add("gtinSODKCPM56hbuy9823");
            Secrets.Add("KEJDHIUW75dvbnj234522");
        }
        public UserUtilities(IUserRepository iUserRepository)
        {
            UserRepo = iUserRepository;
        }
        internal IUserRepository UserRepo { get; set; }

        internal bool IsValidRequest(HttpRequest request)
        {
            StringValues headerValues;
            if (request.Headers.TryGetValue("secret", out headerValues))
            {
                string secret = headerValues.First();
                if (!string.IsNullOrEmpty(secret))
                {
                    foreach (string key in Secrets)
                    {
                        if (key == secret)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal bool ValidateUser(string userName,string password)
        {
            User aUser = UserRepo.Find(userName);
            if (aUser == null) return false;
            if (aUser.Password == password.Trim())
            {
                return true;
            }
            return false;
        }

        internal UserToken GetAccessTokenForUser(string userName)
        {
            UserToken aToken;
            UserTokens.TryGetValue(userName,out aToken);
            if (aToken != null && aToken.ValidTill > DateTime.Now) return aToken;
            return AddNewAccessToken(userName);
        }

        private UserToken AddNewAccessToken(string userName)
        {
            UserToken aToken = new UserToken();
            aToken.UserName = userName;
            aToken.AccessToken = GetNewToken();
            aToken.CreatedAt = DateTime.Now;
            aToken.ValidTill = DateTime.Now.AddHours(5);
            UserTokens.TryAdd(userName, aToken);
            return aToken;
        }

        internal UserToken RemoveAccessToken(string accessToken)
        {
            string userName=null;
            var allTokens = UserTokens.Values;
            foreach (UserToken aToken in allTokens)
            {
                if (aToken.AccessToken == accessToken)
                {
                    userName = aToken.UserName;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(userName))
            {
                UserToken oToken;
                UserTokens.TryRemove(userName, out oToken);
                return oToken;
            }
            return null;
        }

        internal User GetUserDetailsFromAccessToken(string accessToken)
        {
            var allTokens=UserTokens.Values;
            foreach(UserToken aToken in allTokens)
            {
                if (aToken.AccessToken == accessToken)
                {
                    if(aToken.ValidTill > DateTime.Now)
                    {
                        User aUser = UserRepo.Find(aToken.UserName);
                        if (aUser != null) return aUser;
                    }
                }
            }
            return null;
        }

        private static string GetNewToken()
        {
            Guid guidValue = Guid.NewGuid();
            MD5 md5 = MD5.Create();
            return new Guid(md5.ComputeHash(guidValue.ToByteArray())).ToString().Replace("/", "_").Replace("+", "_");
        }
    }
}
