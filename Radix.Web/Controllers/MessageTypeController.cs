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
    public class MessageTypeController : Controller
    {
        private readonly IMessageTypeService _messageTypeService;
        private readonly IMapper _mapper;

        public MessageTypeController(IMessageTypeService messageTypeService, IMapper mapper)
        {
            this._messageTypeService = messageTypeService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            MessageTypeViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _messageTypeService.GetById(Id);
                viewModel = _mapper.Map<MessageTypeViewModel>(obj);
            }
            var model = Id == 0 ? new MessageTypeViewModel() : viewModel;
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody]MessageTypeViewModel viewModel)
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

            var obj = _mapper.Map<MessageType>(viewModel);

            if (_messageTypeService.Save(obj, ref message))
            {
                return Json(new { success = true, reason = string.Empty });
            }
            else
            {
                return Json(new { success = false, reason = message });
            }
        }

        public IActionResult Edit(int Id)
        {
            MessageTypeViewModel result = new MessageTypeViewModel();
            result.Id = Id;
            return PartialView("Create", result);
        }

        public IActionResult EditData(int Id)
        {
            MessageTypeViewModel result = null;
            if (Id > 0)
            {
                MessageType obj = _messageTypeService.GetById(Id);
                result = _mapper.Map<MessageTypeViewModel>(obj);
            }
            else
            {
                result = new MessageTypeViewModel();
            }

            return Json(new { messageType = result });
        }

        public ActionResult GetAll()
        {
            var all = _messageTypeService.GetAll().ToList();
            var list = _mapper.Map<List<MessageTypeViewModel>>(all);
            return Json(new { messageTypes = list });
        }

        public IActionResult Data(IDataTablesRequest requestModel)
        {
            DataTablesResponse response = _messageTypeService.SearchApi(requestModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}