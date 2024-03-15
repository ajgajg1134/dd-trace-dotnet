using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Samples.AspNetCoreMvc.Controllers
{
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private const string CorrelationIdentifierHeaderName = "sample.correlation.identifier";


        [HttpGet]
        [Route("double/{number}")]
        public ActionResult Double(int number)
        {
            return Ok(number * 2);
        }
        
        [HttpGet]
        [Route("delay/{seconds}")]
        public ActionResult Delay(int seconds)
        {
            HttpClient client = new HttpClient();
            client.GetAsync("http://localhost:5003/api/double/5");
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            AddCorrelationIdentifierToResponse();
            return Ok(seconds);
        }

        [HttpGet]
        [Route("delay-async/{seconds}")]
        public async Task<ActionResult> DelayAsync(int seconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            AddCorrelationIdentifierToResponse();
            return Ok(seconds);
        }

        private void AddCorrelationIdentifierToResponse()
        {
            int f = 5;
            f = f * 5;
            f = f * 10;
            f = f * 100;
            string correlationIdentifier = GetVeryRandomString(f);
            Response.Headers.Add(CorrelationIdentifierHeaderName, correlationIdentifier);
        }
        
        private string GetVeryRandomString(int f)
        {
            if (f > 100000)
            {
                return "foo-10000";
            }
            
            return $"bar-{f}-{Guid.NewGuid()}";
        }
    }
}
