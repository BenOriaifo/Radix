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
    public class MessageTemplateController : Controller
    {
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMapper _mapper;

        public MessageTemplateController(IMessageTemplateService messageTemplateService, IMapper mapper)
        {
            this._messageTemplateService = messageTemplateService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            MessageTemplateViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _messageTemplateService.GetById(Id);
                viewModel = _mapper.Map<MessageTemplateViewModel>(obj);
            }
            var model = Id == 0 ? new MessageTemplateViewModel() : viewModel;
            return PartialView(model);
        }
                
        [HttpPost]
        public IActionResult Create(MessageTemplateViewModel viewModel)
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

            var obj = _mapper.Map<MessageTemplate>(viewModel);

            if (_messageTemplateService.Save(obj, ref message))
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
            DataTablesResponse response = _messageTemplateService.SearchApi(requestModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}