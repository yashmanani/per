using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication1.IServices;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IExcelParserService _ExcelParserService { get; }
        public ProductController(IExcelParserService excelParserService)
        {
            _ExcelParserService = excelParserService;
        }

        [HttpPost]
        public FileContentResult Post([FromBody] JsonElement json)
        {
            return File(_ExcelParserService.Parse(json), MimeTypes.GetMimeType("data.xlsx"), "data.xlsx"); //, "application/force-download", "data.xlsx");    //,MimeTypes.GetMimeType("data.xlsx"));
        }

        [HttpPost("excel")]
        public IActionResult PostData([FromBody] JsonElement json)
        {
            return Ok(_ExcelParserService.Parse1(json));
        }
    }
}
