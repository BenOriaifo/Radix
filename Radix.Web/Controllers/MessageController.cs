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
using System.Net;
using Newtonsoft.Json;

namespace Radix.Web.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, IMapper mapper)
        {
            this._messageService = messageService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Search()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            MessageViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _messageService.GetById(Id);
                viewModel = _mapper.Map<MessageViewModel>(obj);
            }
            var model = Id == 0 ? new MessageViewModel() : viewModel;
            return PartialView(model);
        }

        public IActionResult Edit(long Id)
        {
            MessageViewModel result = new MessageViewModel();
            result.Id = Id;
            return PartialView("Create", result);
        }

        public IActionResult EditData(long Id)
        {
            MessageViewModel result = null;
            if(Id > 0)
            {
                Message obj = _messageService.GetById(Id);
                result = _mapper.Map<MessageViewModel>(obj);
            }
            else
            {
                result = new MessageViewModel();
            }

            return Json(new { message = result });
        }

        [HttpPost]
        public IActionResult Create([FromBody]MessageViewModel message)
        {
            string msg = string.Empty;

            string validationErrors = string.Join(",", ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, reason = "Validation Failed. \n" + validationErrors });
            }
            message.MessageCode = "REGIS";
            var obj = _mapper.Map<Message>(message);

            if (_messageService.Save(obj, ref msg))
            {
                return Json(new { success = true, reason = string.Empty });
            }
            else
            {
                return Json(new { success = false, reason = message });
            }
        }

        public IActionResult SendMessage(long Id)
        {
            string msg = string.Empty;
            bool success = false;
            if (Id > 0)
            {
                Message obj = _messageService.GetById(Id);
                if (_messageService.SendMessage(obj, ref msg))
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
            }
            return Json(new { success = success});
        }        

        public IActionResult Data(IDataTablesRequest requestModel, AdvancedSearchViewModel searchViewModel)
        {
            DataTablesResponse response = _messageService.SearchApi(requestModel, searchViewModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}