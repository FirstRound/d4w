using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using D4w_cross.Models.API;
using Newtonsoft.Json;
using D4w_cross.Models.API.Search;
using System.IO;

namespace D4w_cross.Helpers
{

    class APIHelper
    {
        const String URL = "https://d4w-api.herokuapp.com/";

        protected String SendPost(String path, String data)
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.Headers.Add(HttpRequestHeader.UserAgent, "BeTripXamarinApp");
            var token = DBHelper.Instance.GetToken();
            if (token != null)
                client.Headers.Add(HttpRequestHeader.Authorization, token.TokenStr);
            try
            {
                return client.UploadString(URL + path, "POST", data);
            }catch (Exception e)
            {
                throw e;
            }
        }

        protected String SendPut(String path, String data)
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            var token = DBHelper.Instance.GetToken();
            if (token != null)
                client.Headers.Add(HttpRequestHeader.Authorization, token.TokenStr);

            try
            {
                return client.UploadString(URL + path, "PUT", data);
            }catch (Exception e)
            {
                throw e;
            }
        }

        protected  String SendGet(String path, List<Tuple<String, String>> query) 
        {
            var client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            var token = DBHelper.Instance.GetToken();
            if (token != null)
                client.Headers.Add(HttpRequestHeader.Authorization, token.TokenStr);

            foreach (var param in query) {
                client.QueryString.Add(param.Item1, param.Item2);
            }
            try
            {
                return client.DownloadString(URL + path);
            }catch (Exception e)
            {
                throw e;
            }
        }

        protected  void FormErrorResponse(WebException ex, APIResponse obj)
        {
            if (ex.Response != null)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                obj.Errors = JsonConvert.DeserializeObject<Dictionary<String, List<String>>>(resp);
                obj.Status = response.StatusCode;
            }
            else
            {
                obj.Status = HttpStatusCode.ServiceUnavailable;
            }
        }
    }
}
