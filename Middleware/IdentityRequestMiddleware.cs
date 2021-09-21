using CARAPI.Data;
using IdentityServer4.Services.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer4.Middleware
{
    public class IdentityRequestMiddleware
    {
        private readonly ILogger<IdentityRequestMiddleware> _logger;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;

        public IdentityRequestMiddleware(
            ILogger<IdentityRequestMiddleware> logger,
            ITokenService _tokenService,
            IConfiguration _config,
            RequestDelegate _next,
            IServiceScopeFactory _scopeFactory,
            IHttpContextAccessor _httpContextAccessor,
            IHttpClientFactory _clientFactory)
        {
            _logger = logger;
            this._tokenService = _tokenService;
            this._config = _config;
            this._next = _next;
            this._scopeFactory = _scopeFactory;
            this._httpContextAccessor = _httpContextAccessor;
            this._clientFactory = _clientFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            string path = httpContext.Request.Path.Value;

            if (path.ToLower() == "/swagger/index.html" || path.ToLower() == "/swagger/v1/swagger.json" || path.ToLower() == "/register" || path.ToLower() == "/login")
            {
                await _next(httpContext);
            }
            else
            {
                //await _tokenService.IsTokenValid("", "", "");
                string token = _httpContextAccessor.HttpContext.Request.Headers.Where(x => x.Key == "Authorization").FirstOrDefault().Value.FirstOrDefault().ToString();

                JwtPayload jwtPayload = _tokenService.GetJwtPayload(token.Replace("Bearer ", ""));
                string client_id = string.Empty;
                string user_id = string.Empty;
                string scope = string.Empty;
                int status = 0;


                if (jwtPayload != null && jwtPayload.Any())
                {
                    client_id = jwtPayload.Where(x => x.Key == "client_id").FirstOrDefault().Value.ToString();
                    scope = jwtPayload.Where(x => x.Key == "scope").FirstOrDefault().Value.ToString();
                    if (!string.IsNullOrEmpty(client_id))
                    {
                        var httpClient = new HttpClient();
                        var url = ("https://localhost:5000/Api/Auth/GetApplicationUserByUserName?username=" + client_id);
                        var response = await httpClient.GetAsync(url);
                        var stringContent = await response.Content.ReadAsStringAsync();
                        ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(stringContent);
    
                        if (user != null)
                        {
                            status = user.IsActive == true ? 1 : 0;
                            client_id = user.SubjectId;
                            user_id = user.Id;
                        }
                    }                    
                }

                bool res = await _tokenService.SaveAccessLog(status, user_id, client_id );
                if(status == 1 && res == true)
                {
                    await _next(httpContext);
                }
                else
                {
                    httpContext.Response.ContentType = "text/plain";
                    httpContext.Response.StatusCode = 29; //UnAuthorized
                    //httpContext.Response.Redirect("");
                }
            }
        }
    }
}
