using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZingTickets.API.Controllers;

namespace ZingTickets.UserManagement.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : BaseController
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProvider;
        private readonly CognitoUserPool _cognitoUserPool;

        public AuthController()
        {
            _cognitoIdentityProvider = new AmazonCognitoIdentityProviderClient(RegionEndpoint.USEast1); ;
            _cognitoUserPool = new CognitoUserPool("us-east-1_b7tOkzk2k", "7mvigecpn9tepkdooumkanjq14", _cognitoIdentityProvider, "18081j6393m3bb97p9i2se46l9pakd4j4tenmlqlg1av4m4ajo0r");
        }

        [HttpGet("Login")]
        public async Task<IActionResult> LoginAsync(string emailId)
        {
            var user = new CognitoUser(emailId, "7mvigecpn9tepkdooumkanjq14", _cognitoUserPool, _cognitoIdentityProvider, clientSecret: "18081j6393m3bb97p9i2se46l9pakd4j4tenmlqlg1av4m4ajo0r");

            var authRequest = new InitiateSrpAuthRequest()
            {
                Password = "Welcome@88"
            };

            var response = await user.StartWithSrpAuthAsync(authRequest);

            var timeSpan = TimeSpan.FromHours(5);
            var expires = DateTimeOffset.Now + timeSpan;
            var result = new TokenResponseModel
            {
                expires = expires,
                accessToken = response.AuthenticationResult.AccessToken,
                refreshToken = response.AuthenticationResult.RefreshToken
            };
            return PackageData(result, HttpStatusCode.OK);
        }
    }
}
