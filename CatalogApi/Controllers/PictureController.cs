using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController:ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        public PictureController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        [Route("{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var webRoot = _env.ContentRootPath + "\\wwwroot\\";
            var path = Path.Combine(webRoot + "/Pictures/", fileName);
            if (!System.IO.File.Exists(path))
            {
                path = Path.Combine(webRoot + "/Pictures/", "0.png");
            }
            var buffer = System.IO.File.ReadAllBytes(path);
            return File(buffer, "image/png");
        }
    }
}
