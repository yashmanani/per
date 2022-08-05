using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.IServices;
using WebApplication1.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;

namespace WebApplication1.Services
{
    public class ExcelParserService : IExcelParserService
    {
        private CSharpExternalContext _DbContext { get; }
        private string _Connection { get; }
        public ExcelParserService(CSharpExternalContext CSharpExternalContext, IConfiguration configuration)
        {
            _DbContext = CSharpExternalContext;
            _Connection = configuration.GetConnectionString("con");
        }

        public byte[] Parse(JsonElement jsonInput)
        {
            //DataTable dt = new DataTable();
            //using (SqlConnection con = new SqlConnection(_Connection))
            //{
            //    SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Product", con);
            //    con.Open();
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da.Fill(dt);
            //    con.Close();
            //    string JSONString = string.Empty;
            //    JSONString = JObject.FromObject(dt).ToString();
            //    return new OkObjectResult(JSONString);
            //}

            //var data = _DbContext.Products.ToList();
            ////var json = JObject.FromObject(data);
            ////var json1 = json.ToString();
            //var obj = JsonConvert.SerializeObject(data);

            var lines = new List<string>();
            //var json = JObject.FromObject(jsonInput).ToString();
            //var json = jsonInput.GetString();
            var json = jsonInput.ToString();
            DataTable datatable = JsonConvert.DeserializeObject<DataTable>(json);
            var cols = datatable.Columns.Count;
            var rows = datatable.Rows.Count;
            var columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var values = datatable.AsEnumerable().Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(values);
            var linesString = string.Join("\n", lines);
            byte[] bytes = Encoding.ASCII.GetBytes(linesString);
            return bytes;
            //File.WriteAllLines(@"D:/Export.csv", lines);
            //var details = objj.GetType().GetProperties().ToList(); ;
            //foreach (var item in details)
            //{

            //}
            //return new OkObjectResult("done");
        }
        //public FileContentResult GetCsvFile()
        //{

        //}

        public IActionResult ParseToExcel(JsonElement jsonElement)
        {
            var json = jsonElement.ToString();
            DataTable datatable = JsonConvert.DeserializeObject<DataTable>(json);

            var excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Add();
            
            Excel._Worksheet worksheet = (Excel.Worksheet)excelApp.ActiveSheet;

            var cols = datatable.Columns.Count;
            var rows = datatable.Rows.Count;

            var columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

            worksheet.Cells[1] = columnNames;

            //Excel.Range

            return new OkObjectResult("");
        }

        public string Parse1(JsonElement jsonElement)
        {

            Excel.Application excel;
            Excel.Workbook excelworkBook;
            Excel.Worksheet excelSheet;
            Excel.Range excelCellrange;

            var json = jsonElement.ToString();
            DataTable datatable = JsonConvert.DeserializeObject<DataTable>(json);
            var cols = datatable.Columns.Count;
            var rows = datatable.Rows.Count;
            var columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

            excel = new Excel.Application();
            excel.Visible = true;
            excel.DisplayAlerts = false;
            excel.Workbooks.Add();

            excelworkBook = excel.Workbooks.Add(Type.Missing);

            excelSheet = (Excel.Worksheet)excelworkBook.ActiveSheet;
            excelSheet.Name = "Test work sheet";

            excelSheet.Cells[1] = columnNames;

            excelCellrange = excelSheet.Range[excelSheet.Cells[1], excelSheet.Cells[rows, cols]];
            excelCellrange.EntireColumn.AutoFit();
            object misvalue = System.Reflection.Missing.Value;

            excelworkBook.SaveAs(@"D:\data.xlsx", misvalue, misvalue, misvalue, misvalue, misvalue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, misvalue, misvalue, misvalue, misvalue, misvalue);
            excelworkBook.Close(true,misvalue,misvalue);
            excelSheet = null;
            excel.Quit();
            excel = null;

            return "done";
        }
    }
}

