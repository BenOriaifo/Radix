using Microsoft.Extensions.DependencyInjection;
using Radix.Service.Abstract;
using Radix.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Web.Configuration
{
    public class ServiceConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IAdhocMessageService, AdhocMessageService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageTypeService, MessageTypeService>();
            services.AddScoped<IMessageTemplateService, MessageTemplateService>();
            services.AddScoped<INotificationPreferrenceService, NotificationPreferrenceService>();
            services.AddScoped<INotificationSubscriptionService, NotificationSubscriptionService>();
            services.AddScoped<IServiceConfigurationService, ServiceConfigurationService>();
        }
    }
}
