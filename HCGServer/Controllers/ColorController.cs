using System;
using Microsoft.AspNetCore.Mvc;

namespace HCGServer.Controllers
{
    [Route("api/[controller]")]
    public class ColorController : Controller
    {
        // GET: api/Values
        [HttpGet]
        public string GetDCombo(string IC)
        {
            throw new NotImplementedException();
        }

        // GET: api/Values/Precise
        [HttpGet("/Precise")]
        public string GetPCombo(DetailedRequestModel DRM)
        {
            throw new NotImplementedException();
        }

        public class DetailedRequestModel
        {
            public string InputCode { get; set; }
            public double Precision { get; set; } = 1.0;
        }

    }
}
