using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ShopApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<OrderController> _logger;
        private readonly HttpClient _client = new HttpClient();

        public OrderController(IConfiguration config, ILogger<OrderController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var id = Guid.NewGuid().ToString();

            // some logic...

            // send notification to customer
            
            // prepare payload
            var stringPayload = JsonConvert.SerializeObject(new { orderId = id });
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            // get URL of Notifier API from configuration
            var notifierUrl = _config.GetValue<string>("NotifierUrl");

            try
            {
                // send a request to Notifier API
                var msg = await _client.PostAsync(notifierUrl, httpContent);

                // process a response
                if (msg.StatusCode != System.Net.HttpStatusCode.OK)
                    return BadRequest("Can't call Notifier");

                return new JsonResult(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
