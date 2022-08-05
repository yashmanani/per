using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication1.IServices
{
    public interface IExcelParserService
    {
        public byte[] Parse(JsonElement json);
        public string Parse1(JsonElement jsonElement);
    }
}
