using System;
using System.Linq;
using System.Collections.Generic;
using HCGServer.Services.ColorMath;
using HCGServer.Services.HexConverter;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCGServer.ApiController
{
    [Produces("application/json")]
    [Route("api/color")]
    public class ColorController : Controller
    {
        private readonly IHexConverter HCS;
        private readonly ILogger _logger;
        private readonly IColorMath CMS;
        public ColorController(IHexConverter _HCS, IColorMath _CMS, ILoggerFactory loggerFactory) 
        {
            CMS = _CMS;
            HCS = _HCS;
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        // GET: api/color/string
        [HttpGet]
        public IEnumerable<string> GetCombo(string IC)
        {
            Vector<double> IV = string.IsNullOrEmpty(IC) ? HCS.RRGBVector() : HCS.Hex2Vector(IC);
            if (IV == null) { 
                _logger.LogWarning("Denied a Combo-Request, due to flawed input parameters!");
                return null; 
            } else {
                Tuple<bool, Vector<double>> RE = CMS.GetCombo(IV, 255);
                return RE.Item1 ? new List<string>() { HCS.Vector2Hex(IV), HCS.Vector2Hex(RE.Item2) } : new List<string>(2);
            }
        }

        // GET: api/color/precise/DetailedRequestModel
        [HttpGet("/precise")]
        public IEnumerable<string> GetPreciseCombo(DetailedRequestModel DRM)
        {
            if (DRM != null) {
                Vector<double> IV = string.IsNullOrEmpty(DRM.InputCode) ? HCS.RRGBVector() : HCS.Hex2Vector(DRM.InputCode);
                if (IV == null) { return null; } else {
                    Tuple<bool, Vector<double>> RE = CMS.GetCombo(IV, DRM.Precision);
                    return RE.Item1 ? new List<string>() { HCS.Vector2Hex(IV), HCS.Vector2Hex(RE.Item2) } : new List<string>(2);
                }
            } else { 
                _logger.LogWarning("Denied a Precision-Combo-Request, due to flawed input parameters!");
                return new List<string>(2); 
            }
        }

        [HttpPost("/upvote")]
        public IActionResult Upvote(IEnumerable<string> Combo)
        {
            if (Combo != null && Combo.Count() == 2) {
                return Ok();
            } else {
                return BadRequest();
            }
        }

        public class DetailedRequestModel
        {
            public string InputCode { get; set; } = string.Empty;
            public double Precision { get; set; } = 255.0;
        }

    }
}
