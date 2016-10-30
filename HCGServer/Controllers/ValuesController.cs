using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCGServer.Controllers
{
    [Route("api/[controller]")]
    public class ColorController : Controller
    {   
        private ILogger _logger { get; }

        public ColorController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
        
        [HttpGet]
        public string GetPartner(string hex)
        {
            return string.Empty;
        }
    }
}
