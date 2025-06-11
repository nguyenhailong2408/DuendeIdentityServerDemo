using Duende.IdentityServer;
using System.Security.Claims;
namespace DemoIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            builder.Services.AddControllersWithViews();


            builder.Services.AddIdentityServer(options =>
            {

            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(new List<Duende.IdentityServer.Test.TestUser>
                {
                    new Duende.IdentityServer.Test.TestUser
                    {
                        SubjectId = "1",
                        Username = "longnh",
                        Password = "123456",
                        Claims =
                        {
                            new Claim("name", "Longnh"),
                            new Claim("email", "alice@example.com")
                        }
                    },
                    new Duende.IdentityServer.Test.TestUser
                    {
                        SubjectId = "2",
                        Username = "longnh1",
                        Password = "123456",
                        Claims =
                        {
                            new Claim("name", "Longnh"),
                            new Claim("email", "alice@example.com")
                        }
                    }
                })
                .AddDeveloperSigningCredential();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}