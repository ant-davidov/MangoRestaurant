using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IBaseServices : IDisposable
    {
        ResponseDTO ResponseModel {get;set;}
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}