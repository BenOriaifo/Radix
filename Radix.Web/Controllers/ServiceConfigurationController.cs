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
    public class ServiceConfigurationController : Controller
    {
        private readonly IServiceConfigurationService _serviceConfigurationService;
        private readonly IMapper _mapper;

        public ServiceConfigurationController(IServiceConfigurationService serviceConfigurationService, IMapper mapper)
        {
            this._serviceConfigurationService = serviceConfigurationService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        public IActionResult Create(long Id)
        {
            ServiceConfigurationViewModel viewModel = null;
            if (Id > 0)
            {
                var obj = _serviceConfigurationService.GetById(Id);
                viewModel = _mapper.Map<ServiceConfigurationViewModel>(obj);
            }
            var model = Id == 0 ? new ServiceConfigurationViewModel() : viewModel;
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ServiceConfigurationViewModel viewModel)
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

            var obj = _mapper.Map<ServiceConfiguration>(viewModel);

            if (_serviceConfigurationService.Save(obj, ref message))
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
            ServiceConfigurationViewModel result = new ServiceConfigurationViewModel();
            result.Id = Id;
            return PartialView("Create", result);
        }

        public IActionResult EditData(long Id)
        {
            ServiceConfigurationViewModel result = null;
            if (Id > 0)
            {
                ServiceConfiguration obj = _serviceConfigurationService.GetById(Id);
                result = _mapper.Map<ServiceConfigurationViewModel>(obj);
            }
            else
            {
                result = new ServiceConfigurationViewModel();
            }

            return Json(new { serviceConfiguration = result });
        }

        public IActionResult Data(IDataTablesRequest requestModel)
        {
            DataTablesResponse response = _serviceConfigurationService.SearchApi(requestModel);
            DataTablesResponse responseTransformed = DataTablesResponse.Create(requestModel, response.TotalRecords, response.TotalRecordsFiltered, response.Data);
            return new DataTablesJsonResult(responseTransformed, true);
        }
    }
}