namespace Talabat.APIs.Extensions
{
    public static class SwaggerServicesExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static void UseSwaggerMiddelwares(this WebApplication app) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
