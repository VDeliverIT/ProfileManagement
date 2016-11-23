using System;

namespace ProfileManagement.Models
{
    public class UserToken
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }

        //public string IP { get; set; }
        //public string Browser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ValidTill { get; set; }
        
    }
}
