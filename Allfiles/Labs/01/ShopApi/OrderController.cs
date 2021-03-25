using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ShopApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _client = new HttpClient();

        public OrderController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var id = Guid.NewGuid().ToString();

            // some logic...

            // send notification to customer
            var stringPayload = JsonConvert.SerializeObject(new { orderId = id });
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var notifierUrl = _config.GetValue<string>("NotifierUrl");
            var msg = await _client.PostAsync(notifierUrl, httpContent);
            if (msg.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest("Can't call Notifier");

            return new JsonResult(id);
        }
    }
}
