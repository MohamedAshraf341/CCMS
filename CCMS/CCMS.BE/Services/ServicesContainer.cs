using CCMS.BE.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CCMS.BE.Services
{
    public static class ServicesContainer
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IManagementUsersService, ManagementUsersService>();
            services.AddTransient<IMailingService, MailingService>();
            services.AddScoped<OrderService>();
            services.AddScoped<ReasturantService>();
            services.AddScoped<BranchService>();
            services.AddScoped<OrderService>();
            services.AddScoped<PhoneService>();
            services.AddScoped<ClientService>();
            services.AddScoped<MenuItemService>();
        }
    }
}
