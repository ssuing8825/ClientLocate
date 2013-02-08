using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.IO;

namespace ClientLocate.Resources
{
    public class ResourceBase
    {
        protected HttpResponseMessage HandleExcpetion(Exception ex)
        {
            var response = new HttpResponseMessage();
            response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.Content = new StringContent(ex.ToString());
            return response;
        }

        protected HttpResponseMessage CreateResponse(List<string> list, int totalPossible, List<string> listOfWords)
        {
            var response = new HttpResponseMessage();
            StringBuilder sb = new StringBuilder();


            sb.Append("{\"people\":[" + String.Join(",", list.ToArray()) + "]");
            sb.Append(",\"totalPossible\":" + totalPossible.ToString()); ;
            sb.Append(",\"listofwords\":[\"" + String.Join("\",\"", listOfWords.ToArray()) + "\"]"); ;
            sb.Append("}");

            response.StatusCode = HttpStatusCode.OK;
            response.Headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            response.Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
            return response;
        }
    }
}