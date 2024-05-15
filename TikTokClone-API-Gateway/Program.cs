using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace TikTokClone_API_Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("OcelotRoutes.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
           
            //add ocelot
            builder.Services.AddOcelot(builder.Configuration);


            var auth0Domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
            var auth0Audience = builder.Configuration["Auth0:Audience"];

            //This clears the default claim type mapping for JWT tokens. This is necessary because Auth0 uses camelCase claim names, but ASP.NET Core expects PascalCase claim names by default.

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            /* 
            These lines configure authentication services for JWT bearer tokens. It sets the default authentication scheme to JWT bearer authentication and adds JWT bearer authentication for the "Auth0" scheme.
            The Authority property specifies the Auth0 domain as the authority to validate the tokens.
            The Audience property specifies the expected audience for the tokens.
            */

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("viralloop", options =>
            {
                options.Authority = auth0Domain;
                options.Audience = auth0Audience;
            });


            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.MapControllers();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            await app.UseOcelot();

            app.Run();
        }
    }
}
