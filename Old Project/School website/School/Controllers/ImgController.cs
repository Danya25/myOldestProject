using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Zakaz25WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgController : ControllerBase
    {
        [HttpGet("getimg")]
        [Consumes("text/html")]

        public async Task<ActionResult<IEnumerable<string>>> GetImg()
        {
            List<string> links = new List<string> 
            {
                "https://i.ibb.co/jvV2m8b/galery-1.jpg",
                "https://i.ibb.co/bs8d853/galery-2.jpg",
                "https://i.ibb.co/0JJ2kMz/galery-3.jpg",
                "https://i.ibb.co/K76xVgD/galery-5.jpg",
                "https://i.ibb.co/KmqtF1P/galery-4.jpg",
                "https://i.ibb.co/MDc1kdr/galery-6.jpg"
            };
            return links;
        }
    }
}