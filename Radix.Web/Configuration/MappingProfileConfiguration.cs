using AutoMapper;
using Radix.Core.Models;
using Radix.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Web.Configuration
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<AdhocMessageViewModel, AdhocMessage>();
            CreateMap<MessageViewModel, MessageViewModel>();
            CreateMap<MessageTemplateViewModel, MessageTemplate>();
            CreateMap<MessageTypeViewModel, MessageType>();
            CreateMap<NotificationPreferrenceViewModel, NotificationPreferrence>();
            CreateMap<NotificationSubscriptionViewModel, NotificationSubscription>();
            CreateMap<ServiceConfigurationViewModel, ServiceConfiguration>();
        }
    }
}
