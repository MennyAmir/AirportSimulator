using AirportSimulator.data;
using AirportSimulator.services;
using AirportSimulator.services.Interfaces;
using AirportSimulator.services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace AirportSimulator.api
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSingleton<IControlTower, ControlTower>();
            builder.Services.AddScoped<IAirportService, AirportService>();
            builder.Services.AddScoped<IFlightHubs, FlightHubs>();

            builder.Services.AddSignalR();




            builder.Services.AddDbContext<AirportDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            using (var scope = app.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AirportDbContext>();
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }

            app.UseCors(policy => policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed(origin => true)
                            .AllowCredentials());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHub<FlightHubs>("./AirportSimulator.services/Services/FlightHubs");

            app.Run();
        }
    }
}