using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Services.Jwt
{
    public interface ITokenService
    {   
        JwtPayload GetJwtPayload(string token);
        Task<bool> SaveAccessLog(int status, string user_id, string client_id);
    }
}
