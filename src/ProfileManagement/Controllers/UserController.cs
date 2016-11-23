using Microsoft.AspNetCore.Mvc;
using ProfileManagement.Utilities;
using ProfileManagement.Repository;

namespace ProfileManagement.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        UserUtilities UserUtilities { get; set; }
        public UserController(IUserRepository iUserRepository)
        {
            UserUtilities = new UserUtilities(iUserRepository);
        }
       
        [HttpGet]
        [Route("login/{username}/{password}")]
        public IActionResult AuthenticateUser(string userName, string password)
        {
            if(!UserUtilities.IsValidRequest(Request)) return Unauthorized();
            if (string.IsNullOrEmpty(userName)||string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }
            var isValidUser = UserUtilities.ValidateUser(userName, password);
            if (!isValidUser) return BadRequest();
            var accessToken = UserUtilities.GetAccessTokenForUser(userName);
            return new ObjectResult(accessToken);
        }

        [HttpGet]
        [Route("details/accesstoken={accessToken}")]
        public IActionResult GetUserDetailsFromToken(string accessToken)
        {
            if (!UserUtilities.IsValidRequest(Request)) return Unauthorized();
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest();
            }
            var aUser = UserUtilities.GetUserDetailsFromAccessToken(accessToken);
            if (aUser==null) return BadRequest();
            return new ObjectResult(aUser);
        }

        [HttpGet]
        [Route("logout/accesstoken={accessToken}")]
        public IActionResult ExpireAccessToken(string accessToken)
        {
            if (!UserUtilities.IsValidRequest(Request)) return Unauthorized();
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest();
            }
            var aUser = UserUtilities.RemoveAccessToken(accessToken);
            if (aUser == null) return BadRequest();
            return new ObjectResult(aUser);
        }
    }
}
