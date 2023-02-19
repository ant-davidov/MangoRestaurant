using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Web.Services
{
    public class BaseService : IBaseServices
    {
        public ResponseDTO ResponseModel { get;set; }
        public IHttpClientFactory HttpClient {get;set;}
        
        public BaseService(IHttpClientFactory httpClient)
        {
            HttpClient = httpClient;
            ResponseModel = new ResponseDTO();
        }

        

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
           
           try
           {
                var client = HttpClient.CreateClient("MangoAPI");
                HttpRequestMessage  message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if(apiRequest.Data != null)
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        System.Text.Encoding.UTF8,"application/json");
                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default :
                        message.Method = HttpMethod.Get;
                        break;        
                }

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponseDTO;
           }
           catch(Exception e)
           {
                var dto  = new ResponseDTO
                {
                    DesplayMessage = "Error",
                    ErrorMessage = new List<string> { Convert.ToString(e.Message)},
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDTO = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDTO;
           }
        }

        public void Dispose()
        {     
            GC.SuppressFinalize(true);
        }
    }
}