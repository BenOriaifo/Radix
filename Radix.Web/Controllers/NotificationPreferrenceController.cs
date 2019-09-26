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
    public class NotificationPreferrenceController : Controller
    {
        private readonly INotificationPreferrenceService _messageService;
        private readonly IMapper _mapper;

        public NotificationPreferrenceController(INotificationPreferrenceService messageService, IMapper mapper)
        {
            this._messageService = messageService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            NotificationPreferrenceViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _messageService.GetById(Id);
                viewModel = _mapper.Map<NotificationPreferrenceViewModel>(obj);
            }
            var model = Id == 0 ? new NotificationPreferrenceViewModel() : viewModel;
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Create(NotificationPreferrenceViewModel viewModel)
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

            var obj = _mapper.Map<NotificationPreferrence>(viewModel);

            if (_messageService.Save(obj, ref message))
            {
                return Json(new { success = true, reason = string.Empty });
            }
            else
            {
                return Json(new { success = false, reason = message });
            }
        }

        public IActionResult Data(IDataTablesRequest requestModel)
        {
            DataTablesResponse response = _messageService.SearchApi(requestModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}