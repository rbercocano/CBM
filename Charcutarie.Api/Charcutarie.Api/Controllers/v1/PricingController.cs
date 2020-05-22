using System.Linq;
using System.Threading.Tasks;
using Charcutarie.Models;
using Charcutarie.Models.ViewModels;
using Charcutarie.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Charcutarie.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Policy = "ApiAccess", Roles = "Customer,SysAdmin")]
    [ApiController]
    public class PricingController : AuthBaseController
    {
        private readonly IPricingService service;

        public PricingController(IPricingService service)
        {
            this.service = service;
        }

        [HttpPost()]
        public ActionResult<double> CalculatePrice(PriceRequest priceRequest)
        {
            var data = service.CalculatePrice(priceRequest);
            return Ok(data);
        }
    }
}