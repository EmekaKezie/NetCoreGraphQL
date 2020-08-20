using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreGraphQL.Util;

namespace NetCoreGraphQL.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("Cors")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly ILogger<FacilityController> logger;
        //private readonly IHttpContextAccessor accessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_accessor"></param>
        public FacilityController(ILogger<FacilityController> _logger)
        {
            logger = _logger;
        }
        // GET: api/Facility
        [HttpGet]
        public IActionResult Get()
        {
            var ret = (dynamic)null;

            try
            {
                ret = FacilityUtil.GetFacility();
                return Ok(new { result = ret });
            }
            catch (Exception ex)
            {
                //status.Code = ResCode.SYSTEM_ERROR;
                //status.Desc = $"{ResDesc.SYSTEM_ERROR} --> {ex.Message}";
                //ret.Status = status;
                logger.Log(LogLevel.Error, $"[FacilityController][Get] {ex.Message}|{ex.InnerException?.Message}|{ex.InnerException?.InnerException?.Message}");
                return Unauthorized(new { result = ret });
            }
        }

    }
}
