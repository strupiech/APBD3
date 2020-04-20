using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Cw3.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cw3.Handlers
{
    public class BearerAuthHandler
    {
        private readonly IDbService _service;
        public BearerAuthHandler(IDbService service)
        {
            _service = service;
        }

        public async Task<IActionResult> HandleAuthenticateAsync(LoginRequest loginRequest)
        {
            if (!_service.CheckUserCredentials(loginRequest.IndexNumber, loginRequest.Password))
                return new BadRequestResult();
            
            return new AcceptedResult();
        }
    }
}