using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cw3.Services;
using Microsoft.AspNetCore.Http;

namespace Cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IDbService dbService)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                var queryString = httpContext.Request?.QueryString.ToString();
                var method = httpContext.Request.Method.ToString();
                var bodyString = "";

                using (var streamReader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = await streamReader.ReadToEndAsync();
                }

                using (var fileStream = new FileStream("Logs/requestsLog.txt", FileMode.Append))
                {
                    var connectedData = "\n**********\nPath: " + path +
                                        "\nQueryString: " + queryString +
                                        "\nMethod: " + method +
                                        "\nBody: \n" + bodyString;
                    var data = new UTF8Encoding().GetBytes(connectedData);
                    fileStream.Write(data);
                }
                
            }
            
            await _next(httpContext);
        }
    }
}