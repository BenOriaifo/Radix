using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Newtonsoft.Json;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using Radix.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Radix.Service.Concrete
{
    public class MessageService : IMessageService
    {
        IUnitOfWork uow;
        IConfiguration configuration;

        public MessageService(IUnitOfWork uow, IConfiguration config)
        {
            this.uow = uow;
            this.configuration = config;
        }

        public bool Save(Message obj, ref string message)
        {
            if (obj.Id == 0)
            {
                return Add(obj, ref message);
            }
            else
            {
                return Update(obj.Id, obj);
            }
        }

        private bool Add(Message obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            uow.Messages.Add(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }


            return state;
        }

        private bool Update(long Id, Message obj)
        {
            bool state = false;

            var objEx = uow.Messages.Get(obj.Id);
            objEx = obj;
            objEx.Id = Id;
            uow.Messages.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(Message obj)
        {
            bool state = false;

            uow.Messages.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(long id)
        {
            bool state = false;

            var obj = uow.Messages.Get(id);

            uow.Messages.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public Message GetById(long Id)
        {
            return uow.Messages.Get(Id);
        }

        public IEnumerable<Message> GetAll()
        {
            return uow.Messages.GetAll().ToList();
        }

        public bool SendMessage(Message message, ref string msg)
        {
            bool status = false;
            try
            {
                using (var client = new WebClient())
                {
                    var url = configuration["Url"];
                    var Username = configuration["User"];
                    var Password = configuration["Password"];
                    var destination = $"{url}/sms/1/text/query?username={Username}&password={Password}&to={message.MobilePhone}&text={message.SmsMessage}";
                    client.BaseAddress = new Uri(destination).ToString();
                    string response = client.DownloadString(destination);

                    string dataResult = response;
                    SMSUpdateResponseMessageRoot result = JsonConvert.DeserializeObject<SMSUpdateResponseMessageRoot>(dataResult);
                    if (result != null)
                    {
                        message.MessageId = result.results[0].messageId;
                        message.Status = result.results[0].status.name;

                        status = Update(message.Id, message);
                    }
                    return status;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    status = false;
                }
            }
            return status;
        }

        public bool UpdateMessageRequestStatus(Message message)
        {
            bool status = false;

            //using (SqlConnection con = AppConfig.Connection())
            //{
            //    con.Open();
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.Connection = con;
            //        cmd.CommandText = "UPDATE MESSAGE SET IsSent = @IsSent, DateSent = @DateSent, Status = @Status, MessageId = @MessageId WHERE Id = @Id";
            //        cmd.CommandType = System.Data.CommandType.Text;
            //        cmd.Parameters.AddWithValue("@IsSent", 1);
            //        cmd.Parameters.AddWithValue("@DateSent", DateTime.Now);
            //        cmd.Parameters.AddWithValue("@Status", message.Status);
            //        cmd.Parameters.AddWithValue("@MessageId", message.MessageId);
            //        cmd.Parameters.AddWithValue("@Id", message.Id);
            //        int result = cmd.ExecuteNonQuery();
            //        if (result > 0)
            //            status = true;
            //    }
            //}
            return status;
        }


        public DataTablesResponse SearchApi(IDataTablesRequest requestModel, AdvancedSearchViewModel searchViewModel)
        {
            DataTablesResponse response = null;            

            if (searchViewModel.DateExtractedFrom.HasValue || searchViewModel.EmployerName != null || searchViewModel.EmployerCode != null || searchViewModel.DateSentFrom.HasValue)
            {

                //string dateExtracted = (searchViewModel.DateExtractedTo.Value - searchViewModel.DateExtractedFrom.Value).TotalDays.ToString();
                //var extracted = Convert.ToDateTime(dateExtracted);

                //var dateSent = (searchViewModel.DateSentTo.Value - searchViewModel.DateSentFrom.Value).TotalDays.ToString();
                //var sent = Convert.ToDateTime(dateSent);

                IQueryable<Message> searchQuery = uow.Messages.GetAll().Where(m => m.EmployerCode == searchViewModel.EmployerCode && m.EmployerName == searchViewModel.EmployerName).AsQueryable();
                var searchTotalCount = searchQuery.Count();

                var searchFilteredCount = searchQuery.Count();

                // Sorting
                var searchOrderColums = requestModel.Columns.Where(x => x.Sort != null);

                //paging
                var searchData = searchQuery.OrderBy(searchOrderColums).Skip(requestModel.Start).Take(requestModel.Length);

                var searchTransformedData = from tr in searchData
                                            select new
                                            {
                                                Id = tr.Id,
                                                MessageCode = tr.MessageCode,
                                                MobilePhone = tr.MobilePhone,
                                                Email = tr.Email,
                                                PIN = tr.RSAPIN,
                                                Fullname = tr.FullName,
                                                Status = tr.Status,
                                                IsSent = tr.IsSent,
                                                DateExtracted = tr.DateExtracted,
                                                DateSent = tr.DateSent,
                                                EmployerName = tr.EmployerName
                                            };

                response = DataTablesResponse.Create(requestModel, searchTotalCount, searchFilteredCount, searchTransformedData);
            }
            else
            {
                IQueryable<Message> query = uow.Messages.GetAll().AsQueryable();

                var totalCount = query.Count();

                // Apply filters
                if (!string.IsNullOrEmpty(requestModel.Search.Value))
                {
                    var value = requestModel.Search.Value.Trim();
                    query = query.Where(p =>
                                            p.FullName.ToLower().Contains(value.ToLower()) ||
                                            p.RSAPIN.ToLower().Contains(value.ToLower()) ||
                                            p.MobilePhone.ToLower().Contains(value.ToLower()) ||
                                            p.MessageCode.ToLower().Contains(value.ToLower())
                                        //p.Email.ToLower().Contains(value.ToLower())
                                        );
                }

                var filteredCount = query.Count();

                // Sorting
                var orderColums = requestModel.Columns.Where(x => x.Sort != null);

                //paging
                var data = query.OrderBy(orderColums).Skip(requestModel.Start).Take(requestModel.Length);

                var transformedData = from tr in data
                                      select new
                                      {
                                          Id = tr.Id,
                                          MessageCode = tr.MessageCode,
                                          MobilePhone = tr.MobilePhone,
                                          Email = tr.Email,
                                          PIN = tr.RSAPIN,
                                          Fullname = tr.FullName,
                                          Status = tr.Status,
                                          IsSent = tr.IsSent,
                                          DateExtracted = tr.DateExtracted,
                                          EmployerName = tr.EmployerName
                                      };

                response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            }

            return response;
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            throw new System.NotImplementedException();
        }

        public bool SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
