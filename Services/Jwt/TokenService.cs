using CARAPI.Data;
using IdentityServer4.Data.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Services.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string IdentityServerBaseUrl = string.Empty;

        public TokenService(IServiceScopeFactory scopeFactory, 
            IHttpContextAccessor _httpContextAccessor,
            IConfiguration _configuration)
        {
            this.scopeFactory = scopeFactory;
            this._httpContextAccessor = _httpContextAccessor;
            IdentityServerBaseUrl = _configuration["IdentityServer4:Url"];
        }

        public JwtPayload GetJwtPayload(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(token);
            return tokenData.Payload;
        }
        public async Task<bool> SaveAccessLog(int status, string user_id, string client_id)
        {
            IdentiyUserLog log = new IdentiyUserLog();
            try
            {
                var request = _httpContextAccessor.HttpContext;
                var headers = _httpContextAccessor.HttpContext.Request.Headers;

                log = new IdentiyUserLog
                {
                    Method = request?.Request?.Method,
                    Host = request?.Request?.Host.Value,
                    Path = request?.Request?.Path.Value,
                    Protocol = request?.Request?.Protocol,
                    PathBase = request?.Request?.PathBase,
                    IsHttps = (bool)(request?.Request?.IsHttps),
                    Language = headers.Where(x => x.Key == "Accept-Language")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    Authorization = headers.Where(x => x.Key == "Authorization")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    ReferUrl = headers.Where(x => x.Key == "Referer")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    Agent = headers.Where(x => x.Key == "User-Agent")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    RequestDevice = headers.Where(x => x.Key == "sec-ch-ua")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    RequestMobileDevice = headers.Where(x => x.Key == "sec-ch-ua-mobile")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    OS = headers.Where(x => x.Key == "sec-ch-ua-platform")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    FetchSite = headers.Where(x => x.Key == "sec-fetch-site")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    FetchMode = headers.Where(x => x.Key == "sec-fetch-mode")?.FirstOrDefault().Value.FirstOrDefault()?.ToString(),
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    Status = status,
                    UserId = user_id,
                    UserName = client_id                    

                };

                // POST API CALL FROM HERE 

                string jsonObj = JsonConvert.SerializeObject(log);
                StringContent httpContent = new StringContent(jsonObj, System.Text.Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                var url = IdentityServerBaseUrl + "/Api/Log/LoginLogCredentials";
                var response = await httpClient.PostAsync(url, httpContent);
                var stringContent = await response.Content.ReadAsStringAsync();
                var resStatus = JsonConvert.DeserializeObject(stringContent);

                if(Convert.ToInt64(resStatus) == 0)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }

}
