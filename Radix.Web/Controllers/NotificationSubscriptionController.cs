using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radix.Service.Abstract;
using AutoMapper;
using Radix.Core.Models;
using Radix.ViewModel;
using DataTables.AspNet.Core;
using DataTables.AspNet.AspNetCore;

namespace Radix.Web.Controllers
{
    public class NotificationSubscriptionController : Controller
    {
        private readonly INotificationSubscriptionService _service;
        private readonly IMessageTypeService _messageTypeService;
        private readonly IMapper _mapper;

        public NotificationSubscriptionController(INotificationSubscriptionService service, IMapper mapper, IMessageTypeService messageTypeService)
        {
            this._service = service;
            this._mapper = mapper;
            this._messageTypeService = messageTypeService;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            NotificationSubscriptionViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _service.GetById(Id);
                viewModel = _mapper.Map<NotificationSubscriptionViewModel>(obj);
            }
            var model = Id == 0 ? new NotificationSubscriptionViewModel() : viewModel;

            var messageTypes = _messageTypeService.GetAll();
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody]NotificationSubscriptionViewModel notificationSubscription)
        {
            string message = string.Empty;

            string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, reason = "Validation Failed. \n" + validationErrors });
            }

            var obj = _mapper.Map<NotificationSubscription>(notificationSubscription);

            if (_service.Save(obj, ref message))
            {
                return Json(new { success = true, reason = string.Empty });
            }
            else
            {
                return Json(new { success = false, reason = message });
            }
        }

        public IActionResult Edit(long Id)
        {
            NotificationSubscriptionViewModel result = new NotificationSubscriptionViewModel();
            result.Id = Id;
            return PartialView("Create", result);
        }

        public IActionResult EditData(long Id)
        {
            NotificationSubscriptionViewModel result = null;
            if (Id > 0)
            {
                NotificationSubscription obj = _service.GetById(Id);
                result = _mapper.Map<NotificationSubscriptionViewModel>(obj);
            }
            else
            {
                result = new NotificationSubscriptionViewModel();
            }

            return Json(new { notificationSubscription = result });
        }

        public IActionResult Get(long Id)
        {
            NotificationSubscription obj = _service.GetById(Id);
            var result = _mapper.Map<NotificationSubscriptionViewModel>(obj);
            return Json(new { notificationSubscription = result });
        }

        public IActionResult FetchCustomer([FromQuery]string pin)
        {
            List<Customer> customers = new List<Customer>();
            Customer cust1 = new Customer()
            {
                Id = 1,
                Pin = "83873882",
                FullName = "OMOCHO, OBINNA EMMANUEL",
                Mobile = "09038372882",
                Email = "obinna@gmail.com"
            };
            customers.Add(cust1);

            Customer cust2 = new Customer()
            {
                Id = 2,
                Pin = "766378783",
                FullName = "OZOAMAKA, CHIOMA JANE",
                Mobile = "07063636725",
                Email = "chioma@gmail.com"
            };
            customers.Add(cust2);

            Customer cust3 = new Customer()
            {
                Id = 3,
                Pin = "55626256",
                FullName = "ANIFOWOSE, YOMI SUNDAY",
                Mobile = "0806252662",
                Email = "yomi@gmail.com"
            };
            customers.Add(cust3);

            var result = customers.Where(c => c.Pin == pin).First();
            return Json(new { customer = result });
        }

        public IActionResult Data(IDataTablesRequest requestModel)
        {
            DataTablesResponse response = _service.SearchApi(requestModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}