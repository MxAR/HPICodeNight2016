using HCGServer.Services.ColorMath;
using HCGServer.Services.HexConverter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCGServer.APIControllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ColorController : Controller
    {   
        private ILogger _logger { get; }
        private IHexConverter _HCP { get; }
        private IColorMath _CMP { get; }

        public ColorController(ILoggerFactory loggerFactory, IHexConverter HCP, IColorMath CMP)
        {
            _CMP = CMP;
            _HCP = HCP;
            _logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        // GET: api/color/KomboRequestModel
        [HttpGet]
        public string GetPartner(KomboRequestModel KRM)
        {
            if (KRM != null && KRM.IsFilled()) { return null; } else {
                return _HCP.Vector2Hex(_CMP.GetKombo(_HCP.Hex2Vector(KRM.HexCode), KRM.Precision));
            }
        }

        public class KomboRequestModel 
        {
            public int Precision { get; set; }
            public string HexCode { get; set; }

            public bool IsFilled()
            {
                return !(string.IsNullOrEmpty(HexCode));
            }
        }
    }
}
