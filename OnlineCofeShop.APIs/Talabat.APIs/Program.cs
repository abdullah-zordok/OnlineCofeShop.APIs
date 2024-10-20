using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.APIs.Extensions;
using StackExchange.Redis;
using Talabat.Repository.Identity;
using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Services

            builder.Services.AddControllers();
             
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerServices();
            #endregion
            builder.Services.AddDbContext<StoreContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                ) ;

            builder.Services.AddDbContext<AppIdentityDbContext>(OptionsBuilder =>
            {
                OptionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")); 
            }) ;
            builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            //implementing update database (when run the app migrations auto updated)
            //WebApllicationBuilder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            //WebApllicationBuilder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>(); instead of all of this
                
            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration);
          
            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
         
            var app = builder.Build();
            using var scope=app.Services.CreateScope();
            var services=scope.ServiceProvider;
            var _dbContext=services.GetRequiredService<StoreContext>(); 
            var _identityDbContext=services.GetRequiredService<AppIdentityDbContext>();
            var loggerFactory=services.GetRequiredService<ILoggerFactory>();
            try { 
            await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbContext);
                await _identityDbContext.Database.MigrateAsync();
                var _userManager = services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUsersAsync(_userManager);
            }
            catch(Exception ex) {
            var logger=loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has occured during apply thee migration");
            }
            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
               app.UseSwaggerMiddelwares();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            #endregion

            app.Run();
        }
    }
}