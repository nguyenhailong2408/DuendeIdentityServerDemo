using Microsoft.IdentityModel.Tokens;

namespace WebApi1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:44317"; // URL IdentityServer
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                // Can set anyname of AddPolicy ex:ApiScope,...
                // How to use: Add before action of controller (endpoint) [Authorize("ApiScope")], [Authorize("AdminScope")], ...
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });

                //options.AddPolicy("AdminScope", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireClaim("scope", "admin");
                //});

                //options.AddPolicy("ReadOrWrite", policy =>
                //{
                //    policy.RequireAuthenticatedUser();
                //    policy.RequireClaim("scope", "read", "write");
                //});
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //Declare default just one ApiScope. Orthers controller just need override [Authorize("AdminScope")]
            app.MapControllers().RequireAuthorization("ApiScope");
            //app.MapControllers();

            app.Run();
        }
    }
}
