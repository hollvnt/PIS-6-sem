using System;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Lab1b
{
    public class IISHandler : IHttpHandler
    {
        WebSocket socket;

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(WebSocketRequest);
            }
            else
            {
                HttpResponse response = context.Response;
                HttpRequest request = context.Request;

                string[] reqURL = request.Url.ToString().Split('/');
                string file = reqURL[reqURL.Length - 1];

                if (file.CompareTo("") == 0)
                {
                    response.ContentType = "text/html";
                    
                    response.WriteFile("WebSocketPage.html");                         
                }
                else if(Regex.IsMatch(file, @"\.js"))
                {
                    response.ContentType = "text/js";
                    response.WriteFile(file);
                }
            }
          
            
        }

        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            socket = context.WebSocket;
            string s = await Receive();
            await Send(s);
            int i = 0;
            while (socket.State == WebSocketState.Open)
            {
                Thread.Sleep(2000);
                await Send("[" + (i++).ToString() + "]");
            }
        }

        private async Task<string> Receive()
        {
            string rc = null;
            var buffer = new ArraySegment<byte>(new byte[512]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            rc = System.Text.Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

            return rc;
        }

        private async Task Send(string s)
        {
            var sendbuffer = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes("Ответ: " + s));
            await socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
