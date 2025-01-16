namespace BlazorApp18_Server.Services
{
    /// <summary>
    /// For Registering the API Services
    /// </summary>
    public static class ServiceRegistration
    {
        /// <summary>
        /// For Registering the API related Services
        /// </summary>
        /// <param name="services"></param>
        public static void AddApiServices(this IServiceCollection services)
        {
            // Register your services here
            services.AddScoped<ApiServices>();
            services.AddScoped<ProductService>();
            services.AddScoped<ProductCartService>();
        }
    }
}
