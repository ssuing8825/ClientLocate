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
    public class SearchResource : ResourceBase
    {

        [WebGet(UriTemplate = "?q={query}&searchType={searchType}")]
        public HttpResponseMessage Search(string query, string searchType)
        {
            try
            {//Need to use DI
                var searchHelper = new SearchHelper();
                var list = searchHelper.LocateClient(query, searchType);
                var totalPossible = searchHelper.CountClient(query, searchType);
                var listOfWords = searchHelper.GetWordList(query, searchType);
                return CreateResponse(list, totalPossible, listOfWords);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HandleExcpetion(ex));
            }
        }

        [WebGet(UriTemplate = "custom?name={name}&phone={phone}&address={address}&policy={policy}")]
        public HttpResponseMessage SearchByPeice(string name, string phone, string address, string policy)
        {
            try
            {
                //Need to use DI
                var searchHelper = new SearchHelper();
                var list = searchHelper.LocateClient(name, phone, address, policy);
                var totalPossible = searchHelper.CountClient(name, phone, address, policy);

                var listOfWords = new List<string>() {"NA"};
                //var listOfWords = searchHelper.GetWordList(name);
                return CreateResponse(list, totalPossible, listOfWords);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HandleExcpetion(ex));
            }
        }
    }
}