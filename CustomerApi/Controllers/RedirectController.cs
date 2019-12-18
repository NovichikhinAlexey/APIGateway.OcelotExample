using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedirectController : ControllerBase
    {
        // GET api/customer
        [HttpGet]
        public void Google()
        {
            var responce = HttpContext.Response;
            if (responce.Headers.TryGetValue("Location", out _))
                responce.Headers.Remove("Location");

            responce.Headers.Add("Location", "https://google.com");
            responce.StatusCode = (int)HttpStatusCode.Redirect;
        }
    }
}