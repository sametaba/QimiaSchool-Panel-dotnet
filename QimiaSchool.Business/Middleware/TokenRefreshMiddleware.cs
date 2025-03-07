using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;


namespace QimiaSchool.Business.Middleware;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public TokenRefreshMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var token = authorizationHeader["Bearer ".Length..].Trim();
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var expirationTime = jwt.ValidTo;

        if (expirationTime <= DateTime.UtcNow.AddMinutes(10))
        {
            var newToken = await GetNewToken();
            context.Request.Headers["Authorization"] = $"Bearer {newToken}";
        }

        await _next(context);
    }

    private async Task<string> GetNewToken()
    {
        using var client = new HttpClient();
        var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config["Auth0:ClientId"] },
            { "client_secret", _config["Auth0:ClientSecret"] },
            { "audience", _config["Auth0:Audience"] }
        };

        var response = await client.PostAsync($"{_config["Auth0:Domain"]}/oauth/token", new FormUrlEncodedContent(requestBody));
        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
        return tokenData!["access_token"];
    }
}
