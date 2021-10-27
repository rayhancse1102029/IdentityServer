using CARAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CARAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string IdentityServerBaseUrl = string.Empty;

        public AuthController(IConfiguration _configuration)
        {
            IdentityServerBaseUrl = _configuration["IdentityServer4:Url"];
        }

        [Route("Signup")]
        [HttpGet]
        public async Task<IActionResult> Signup(RegisterViewModel model)
        {
            string jsonObj = JsonConvert.SerializeObject(model);
            StringContent httpContent = new StringContent(jsonObj, System.Text.Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            var url = IdentityServerBaseUrl + "/Api/Auth/Registration";
            var response = await httpClient.PostAsync(url, httpContent);
            var stringContent = await response.Content.ReadAsStringAsync();
            var resStatus = JsonConvert.DeserializeObject(stringContent);

            return Ok(resStatus);
        }



        [Route("Signin")]
        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel model)
        {
            var httpClient = new HttpClient();
            var url = IdentityServerBaseUrl + ("/Api/Auth/IdentityUserInfoGetByUsername?username=" + model.Username);
            var response = await httpClient.GetAsync(url);
            var stringContent = await response.Content.ReadAsStringAsync();
            var identityUserInfo = JsonConvert.DeserializeObject<LoginViewModel>(stringContent);

            if(identityUserInfo == null)
            {
                return Ok("Invalid User");
            }
            if(identityUserInfo.IsActive == true && identityUserInfo.EmailConfirmed == true)
            {

                HttpContent content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", identityUserInfo.GrantType),
                    new KeyValuePair<string, string>("scope", identityUserInfo.Scope),
                    new KeyValuePair<string, string>("client_id", identityUserInfo.ClientId),
                    new KeyValuePair<string, string>("client_secret", model.Password)
                });

                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var tokenUrl = IdentityServerBaseUrl + "/connect/token";
                var result = httpClient.PostAsync(tokenUrl, content).Result;
                string strContent = await result.Content.ReadAsStringAsync();
                TokenViewModel res = new TokenViewModel();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(strContent)))
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(TokenViewModel));
                    res = (TokenViewModel)deserializer.ReadObject(ms);

                }

                return Ok(res);
            }
            else
            {
                return Ok(identityUserInfo.IsActive == false ? "Your Account has been blocked" 
                    : "Please Confirm Your Account Which Link Provided in Your Email");
            }            
        }


    }

}
