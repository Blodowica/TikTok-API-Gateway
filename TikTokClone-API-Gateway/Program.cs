using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace TikTokClone_API_Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("OcelotRoutes.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration);



            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.MapControllers();
            await app.UseOcelot();

            app.Run();
        }
    }
}
