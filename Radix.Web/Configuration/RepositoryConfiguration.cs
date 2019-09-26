using Microsoft.Extensions.DependencyInjection;
using Radix.Core;
using Radix.Core.Repositories;
using Radix.Persistence;
using Radix.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Web.Configuration
{
    public class RepositoryConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAdhocMessageRepository, AdhocMessageRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageTypeRepository, MessageTypeRepository>();
            services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();
            services.AddScoped<INotificationPreferrenceRepository, NotificationPreferrenceRepository>();
            services.AddScoped<INotificationSubscriptionRepository, NotificationSubscriptionRepository>();
            services.AddScoped<IServiceConfigurationRepository, ServiceConfigurationRepository>();
        }
    }
}
