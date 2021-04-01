using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Notifier
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Send([FromBody] OrderDto order)
        {
            // some logic...
            throw new Exception("Test exception");

            return new OkObjectResult($"Customer is notified. Order Id: {order.Id}");
        }
    }
}
