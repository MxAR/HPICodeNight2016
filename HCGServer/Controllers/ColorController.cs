using System;
using System.Collections.Generic;
using HCGServer.Services.ColorMath;
using HCGServer.Services.HexConverter;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCGServer.Controllers
{
    [Route("api/[controller]")]
    public class ColorController : Controller
    {
        protected IHexConverter HCS { get; }
        protected ILogger _logger { get; }
        protected IColorMath CMS { get; }
        public ColorController(IColorMath _CMS, IHexConverter _HCS, ILoggerFactory loggerFactory) 
        {
            CMS = _CMS;
            HCS = _HCS;
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        // GET: api/Values
        [HttpGet()]
        public IEnumerable<string> GetCombo(string IC)
        {
            Vector<double> IV = Vector<double>.Build.Dense(3);
            IV = string.IsNullOrEmpty(IC) ? HCS.RRGBVector() : HCS.Hex2Vector(IC);
            if (IV == null) { return null; } else {
                Tuple<bool, Vector<double>> RE = CMS.GetCombo(IV);
                return RE.Item1 ? new List<string>() { HCS.Vector2Hex(IV), HCS.Vector2Hex(RE.Item2) } : null;
            }
        }

        // GET: api/Values/Precise
        [HttpGet("/Precise")]
        public string GetPreciseCombo(DetailedRequestModel DRM)
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
