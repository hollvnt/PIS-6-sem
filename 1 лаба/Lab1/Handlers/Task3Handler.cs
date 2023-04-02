using System;
using System.Web;

namespace Lab1.Handlers
{
    public class Task3Handler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod == "PUT")
            {
                string paramA = context.Request.Params["ParmA"];
                string paramB = context.Request.Params["ParmB"];
                string response = string.Format("PUT-Http-XYZ:ParmA = {0},ParmB = {1}", paramA, paramB);
                context.Response.ContentType = "text/plain";
                context.Response.Write(response);
            }
            else
            {
                context.Response.StatusCode = 405; // Method Not Allowed
                context.Response.StatusDescription = "Invalid HTTP method";
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
