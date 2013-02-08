using System;
using System.ComponentModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using ClientLocate.Models;
using Microsoft.ApplicationServer.Http;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;
using Irony.Compiler;
using Microsoft.ApplicationServer.Http.Dispatcher;

namespace ClientLocate.Resources
{
    [ServiceContract]
    public class CountResource
    {

        SearchHelper searchHelper;

        public CountResource()
        {
            try
            {
                //need DI here
                searchHelper = new SearchHelper();
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
                throw new HttpResponseException(response);

            }
        }

        [WebGet(UriTemplate = "?q={query}&searchType={searchType}")]
        public HttpResponseMessage Search(string query, string searchType)
        {
            var response = new HttpResponseMessage();
            try
            {
                var count = searchHelper.CountClient(query, searchType);
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                response.Content = new StringContent(count.ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
                throw new HttpResponseException(response);
            }
        }

        [WebGet(UriTemplate = "custom?name={name}&phone={phone}&address={address}&policy={policy}")]
        public HttpResponseMessage SearchByPeice(string name, string phone, string address, string policy)
        {
            var response = new HttpResponseMessage();
            try
            {
                var count = searchHelper.CountClient(name, phone, address, policy);
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                response.Content = new StringContent(count.ToString(), Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
                throw new HttpResponseException(response);
            }
        }
    }
}