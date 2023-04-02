using System;
using System.Web;

namespace Lab1.Handlers
{
    public class Task1Handler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            var parmA = request["ParmA"];
            var parmB = request["ParmB"];

            response.AddHeader("Content-Type", "text-plaint");
            response.Write($"Ответ\n" + $"parmA = {(parmA != null ? parmA : "null")}\n" + $"ParmB = {(parmB != null ? parmB : "null")}");
        }
       
    }
}
